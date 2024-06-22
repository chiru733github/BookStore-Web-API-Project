using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IFeedBackRepo
    {
        FeedBackEntity AddFeedBack(int userId, FeedBackModel feedBack);
        FeedBackEntity EditFeedBack(int UserId, int FeedbackId, FeedBackModel feedBack);
        bool RemoveFeedBack(int FeedbackId);
        List<FeedBackEntity> ViewAllFeedBack();
    }
}
