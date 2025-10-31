using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPSS.Service.Services.Interfaces;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Constants;
using System;
using System.Security; // Mặc dù không dùng SecurityException nữa, vẫn nên giữ để tham khảo
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;
        private readonly IReplyRepository _replyRepository;

        public ReplyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _reviewRepository = _unitOfWork.GetRepository<IReviewRepository>();
            _replyRepository = _unitOfWork.GetRepository<IReplyRepository>();
        }

        public async Task<ReplyDto> CreateAsync(Guid userId, ReplyForCreationDto replyDto)
        {
            if (replyDto == null)
                throw new ArgumentNullException(nameof(replyDto), ExceptionMessageConstants.Reply.ReplyDataNull);

            var reviewExists = await _reviewRepository.Entities.AnyAsync(r => r.Id == replyDto.ReviewId);
            if (!reviewExists)
                throw new ArgumentException(ExceptionMessageConstants.Reply.ReviewNotFound, nameof(replyDto.ReviewId));

            var reply = _mapper.Map<Reply>(replyDto);

            reply.Id = Guid.NewGuid();
            reply.CreatedTime = DateTimeOffset.UtcNow;
            reply.UserId = userId;
            reply.CreatedBy = userId.ToString();
            reply.LastUpdatedTime = DateTimeOffset.UtcNow;
            reply.LastUpdatedBy = userId.ToString();
            reply.IsDeleted = false;

            try
            {
                _replyRepository.Add(reply);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Sử dụng hằng số đã có
                throw new ApplicationException(string.Format(ExceptionMessageConstants.Reply.FailedToSave, ex.Message), ex);
            }

            // Lấy lại reply với thông tin User để map DTO đầy đủ
            var createdReply = await _replyRepository.GetSingleAsync(
                predicate: r => r.Id == reply.Id,
                include: q => q.Include(r => r.User)
            );

            return _mapper.Map<ReplyDto>(createdReply);
        }

        public async Task<ReplyDto> UpdateAsync(Guid userId, ReplyForUpdateDto replyDto, Guid id)
        {
            if (replyDto == null)
                throw new ArgumentNullException(nameof(replyDto), ExceptionMessageConstants.Reply.ReplyDataNull);

            var reply = await _replyRepository.GetByIdAsync(id);
            if (reply == null)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Reply.ReplyNotFound, id));

            // [SỬA ĐỔI] Sử dụng hằng số và UnauthorizedAccessException
            if (reply.UserId != userId)
            {
                throw new UnauthorizedAccessException(ExceptionMessageConstants.Reply.NotOwner);
            }

            _mapper.Map(replyDto, reply);
            reply.LastUpdatedTime = DateTimeOffset.UtcNow;
            reply.LastUpdatedBy = userId.ToString();

            _replyRepository.Update(reply);
            await _unitOfWork.SaveChangesAsync();

            var updatedReply = await _replyRepository.GetSingleAsync(
                predicate: r => r.Id == reply.Id,
                include: q => q.Include(r => r.User)
            );

            return _mapper.Map<ReplyDto>(updatedReply);
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var reply = await _replyRepository.GetByIdAsync(id);
            if (reply == null)
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Reply.ReplyNotFound, id));

            // [SỬA ĐỔI] Sử dụng hằng số và UnauthorizedAccessException
            if (reply.UserId != userId)
            {
                throw new UnauthorizedAccessException(ExceptionMessageConstants.Reply.NotOwner);
            }

            // Thay đổi nhỏ: bạn có thể dùng phương thức DeleteByIdAsync để code gọn hơn nếu có
            _replyRepository.Delete(reply);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}