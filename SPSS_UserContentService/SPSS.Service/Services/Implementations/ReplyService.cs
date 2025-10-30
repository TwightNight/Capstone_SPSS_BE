using AutoMapper;
using Microsoft.EntityFrameworkCore; // Cần cho .Include()
using SPSS.Service.Services.Interfaces;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Constants;
using System.Security; // Cần cho SecurityException
using System;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;
        private readonly IReplyRepository _replyRepository;
        // Giả sử bạn có IUserRepository để lấy User,
        // nhưng nếu không, chúng ta có thể dùng Include()

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

            var reply = _mapper.Map<Reply>(replyDto); // Dùng AutoMapper

            reply.Id = Guid.NewGuid();
            reply.CreatedTime = DateTimeOffset.UtcNow;
            reply.UserId = userId;
            reply.CreatedBy = userId.ToString();
            reply.LastUpdatedTime = DateTimeOffset.UtcNow;
            reply.LastUpdatedBy = userId.ToString();
            reply.IsDeleted = false;
            // reply.ReviewId đã được map từ DTO

            try
            {
                _replyRepository.Add(reply);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.Reply.FailedToSave, ex.Message), ex);
            }

            // để AutoMapper có thể map AvatarUrl và UserName
            var createdReply = await _replyRepository.GetSingleAsync(
                predicate: r => r.Id == reply.Id,
                include: q => q.Include(r => r.User) // Giả sử Reply model có navigation "User"
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

            if (reply.UserId != userId)
            {
                // TODO: Thêm NotOwner vào ExceptionMessageConstants.Reply
                throw new SecurityException("You are not the owner of this reply.");
            }

            _mapper.Map(replyDto, reply);
            reply.LastUpdatedTime = DateTimeOffset.UtcNow;
            reply.LastUpdatedBy = userId.ToString();

            _replyRepository.Update(reply);
            await _unitOfWork.SaveChangesAsync();

            // Cần Include User để trả về DTO đầy đủ
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

            if (reply.UserId != userId)
            {
                // TODO: Thêm NotOwner vào ExceptionMessageConstants.Reply
                throw new SecurityException("You are not the owner of this reply.");
            }

            _replyRepository.Delete(reply); // Dùng soft delete nếu nghiệp vụ yêu cầu
            await _unitOfWork.SaveChangesAsync();
        }
    }
}