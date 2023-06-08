using System.Collections.Generic;

namespace LogAppLibrary
{
    public class LogMonitoringModel
    {
        public List<UserModel> User { set; get; } = new List<UserModel>();
        public List<PurposeModel> UserPurpose { set; get; } = new List<PurposeModel>();
        public List<LimitModel> UserLimit { set; get; } = new List<LimitModel>();
    }
}
