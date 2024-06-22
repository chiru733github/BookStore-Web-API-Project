using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class FeedBackBL : IFeedBackBL
    {
        private readonly IFeedBackRepo feedBackRepo;
        public FeedBackBL(IFeedBackRepo repo)
        {
            this.feedBackRepo = repo;
        }
        public FeedBackEntity AddFeedBack(int userId, FeedBackModel feedBack)
        {
            return feedBackRepo.AddFeedBack(userId, feedBack);
        }

        public FeedBackEntity EditFeedBack(int UserId, int FeedbackId, FeedBackModel feedBack)
        {
            return feedBackRepo.EditFeedBack(UserId, FeedbackId, feedBack);
        }

        public bool RemoveFeedBack(int FeedbackId)
        {
            return feedBackRepo.RemoveFeedBack(FeedbackId);
        }

        public List<FeedBackEntity> ViewAllFeedBack()
        {
            return feedBackRepo.ViewAllFeedBack();
        }
    }
}
