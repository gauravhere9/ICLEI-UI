using System.ComponentModel;

namespace WebApp.Global.Constants
{
    public class Enumurations
    {
        public enum GenderTypes
        {
            Male = 1,
            Female = 2,
            Other = 3
        }

        public enum MaritalStatus
        {
            Married = 1,
            Unmarried = 2
        }

        public enum BloodGroupTypes
        {
            [Description("A+")]
            A_Positive = 1,

            [Description("A-")]
            A_Negative = 2,

            [Description("B+")]
            B_Positive = 3,

            [Description("B-")]
            B_Negative = 4,

            [Description("O+")]
            O_Positive = 5,

            [Description("O-")]
            O_Negative = 6,

            [Description("AB+")]
            AB_Positive = 7,

            [Description("AB-")]
            AB_Negative = 8,
        }

        public enum EmailTemplateTypes
        {
            ForgotPassword = 1,
            ChangePassword = 2
        }

        public enum UserTypes
        {
            Admin = 1,
            Student = 2,
            Teacher = 3
        }

        public enum RepoStatusTypes
        {
            Added = 1,
            Updated = 2,
            Deleted = 3,
            RecordExist = 4,
            RecordNotExist = 5,
            Failed = 0,
            TokenExpired = 6,
            RefreshTokenExpired = 7
        }


        public enum HttpMethodTypes
        {
            Get = 1,
            Post = 2,
            Put = 3,
            Patch = 4,
            Delete = 5
        }
    }
}
