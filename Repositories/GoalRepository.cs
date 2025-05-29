using EFDataLayer.DBContext;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class GoalRepository: IGoalRepository
    {
        private readonly BooklyDbContext _context;
        public GoalRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void CreateGoal(Goal goal)
        {
            _context.Goals.Add(goal);
            _context.SaveChanges();
        }

        public List<Goal> GetPersonalGoals(User user)
        {
            return _context.Goals
                .Where(g => g.User.Id == user.Id && g.IsArchived == false).ToList();
        }

        public Goal? GetNewestGoal(bool isIncreasing, User user)
        {
            List<Goal> goals = GetPersonalGoals(user);
            if(isIncreasing) 
                return goals.Where(g => g.Status != Status.Completed && g.Status != Status.Expired).FirstOrDefault();

            if(!isIncreasing) return goals.Where(g => g.Status == Status.In_progress).FirstOrDefault();

            return null;
        }

        public Goal? GetLatestCompletedGoal(User user)
        {
            return _context.Goals
                .Where(g => g.User.Id == user.Id && g.IsArchived == false && g.Status ==Status.Completed).FirstOrDefault();
        }

        public void UpdateProgress(int userId, Goal goal)
        {
            _context.DetachIfTracked(goal, g => g.Id);
             _context.Goals.Update(goal);
            _context.SaveChanges();
        }

        public void UpdateStatus(Status status, int goalId, int userId)
        {
            Goal? goal = _context.Goals.Find(goalId);
            if(goal !=null)
            {
                goal.Status = status;
                _context.Goals.Update(goal);
                _context.SaveChanges();
            }
        }

        public void RemoveGoal(int id)
        {
            Goal? goal = _context.Goals.Find(id);
            if (goal != null)
            {
                goal.IsArchived = true;
                _context.Goals.Update(goal);
                _context.SaveChanges();
            }
        }

        public async Task<List<Goal>> GetAllGoals()
        {
            return _context.Goals.ToList();
        }
    }
}
