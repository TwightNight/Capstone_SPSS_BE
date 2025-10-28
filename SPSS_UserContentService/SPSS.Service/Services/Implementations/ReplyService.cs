using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPSS.Service.Interfaces;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SPSS.Service.Implementations
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly IReviewRepository _reviewRepository;
		private readonly IReplyRepository _replyRepository;

		public ReplyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_reviewRepository = _unitOfWork.GetRepository<IReviewRepository>();
			_replyRepository = _unitOfWork.GetRepository<IReplyRepository>();
		}
        public async Task<ReplyDto> CreateAsync(Guid userId, ReplyForCreationDto replyDto)
        {
            if (replyDto == null)
                throw new ArgumentNullException(nameof(replyDto), "Reply data cannot be null.");

            // Check if the reviewId exists
            var reviewExists = await _reviewRepository.Entities.AnyAsync(r => r.Id == replyDto.ReviewId);
            if (!reviewExists)
                throw new ArgumentException("The specified reviewId does not exist.", nameof(replyDto.ReviewId));

            // Manual mapping of Reply entity
            var reply = new Reply
            {
                Id = Guid.NewGuid(),
                ReviewId = replyDto.ReviewId,
                ReplyContent = replyDto.ReplyContent,
                CreatedTime = DateTimeOffset.UtcNow,
                UserId = userId,
                CreatedBy = userId.ToString(),
                LastUpdatedTime = DateTimeOffset.UtcNow,
                LastUpdatedBy = userId.ToString(),
                IsDeleted = false
            };

			// Add the reply to the database
			_replyRepository.Add(reply);
            await _unitOfWork.SaveChangesAsync();

            // Manual mapping of ReplyDto for return
            var replyDtoResult = new ReplyDto
            {
                Id = reply.Id,
                ReplyContent = reply.ReplyContent,
            };

            return replyDtoResult;
        }

        public async Task<ReplyDto> UpdateAsync(Guid userId, ReplyForUpdateDto replyDto, Guid id)
        {
            if (replyDto == null)
                throw new ArgumentNullException(nameof(replyDto), "Reply data cannot be null.");

            var reply = await _replyRepository.GetByIdAsync(id);
            if (reply == null)
                throw new KeyNotFoundException($"Reply with ID {id} not found.");

            _mapper.Map(replyDto, reply);
            reply.LastUpdatedTime = DateTimeOffset.UtcNow;
            reply.LastUpdatedBy = userId.ToString();
            _replyRepository.Update(reply);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReplyDto>(reply);
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var reply = await _replyRepository.GetByIdAsync(id);
            if (reply == null)
                throw new KeyNotFoundException($"Reply with ID {id} not found.");

            _replyRepository.Delete(reply);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
