namespace StepFly.Services.XiaoMi
{
    public class XiaoMiLoginModel
    {
        public string UserPhone { get; set; }
        public string Password { get; set; }
    }

    #region XiaoMiLoginSuccessModel
    public class XiaoMiLoginSuccessModel
    {
        public Token_Info token_info { get; set; }
        public Regist_Info regist_info { get; set; }
        public Thirdparty_Info thirdparty_info { get; set; }
        public string result { get; set; }
        public Domain domain { get; set; }
        public Domain1[] domains { get; set; }
    }

    public class Token_Info
    {
        public string login_token { get; set; }
        public string app_token { get; set; }
        public string user_id { get; set; }
        public int ttl { get; set; }
        public int app_ttl { get; set; }
    }

    public class Regist_Info
    {
        public int is_new_user { get; set; }
        public long regist_date { get; set; }
        public string region { get; set; }
        public string country_code { get; set; }
    }

    public class Thirdparty_Info
    {
        public string nickname { get; set; }
        public string icon { get; set; }
        public string third_id { get; set; }
        public string email { get; set; }
    }

    public class Domain
    {
        public string iddns { get; set; }
    }

    public class Domain1
    {
        public string[] cnames { get; set; }
        public string host { get; set; }
    }
    #endregion
}