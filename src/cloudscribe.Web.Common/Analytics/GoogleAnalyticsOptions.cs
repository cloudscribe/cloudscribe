namespace cloudscribe.Web.Common.Analytics
{
    public class GoogleAnalyticsOptions
    {
        /// <summary>
        /// you can set this as flase if your privacy plicy does not allow it.
        /// </summary>
        public bool TrackUserId { get; set; } = true;

        public int UserIdDimensionIndex { get; set; } = 1;
        public int RegisteredUserDimensionIndex { get; set; } = 2;
        public int LoginRegisterSourceDimensionIndex { get; set; } = 3;

        public int RegisterSuccessMetricIndex { get; set; } = 1;
        public int RegisterFailMetricIndex { get; set; } = 2;
        public int RegisterSubmitMetricIndex { get; set; } = 3;
        public int LoginSuccessMetricIndex { get; set; } = 4;
        public int LoginFailMetricIndex { get; set; } = 5;
        public int LoginSubmitMetricIndex { get; set; } = 6;

        public string LoginRegisterEventCategory { get; set; } = "Login and Registration";
        public string LoginSuccessEventAction { get; set; } = "Login Success";
        public string LoginFailEventAction { get; set; } = "Login Fail";
        public string LoginSubmitEventAction { get; set; } = "Login Submit";
        public string RegisterSuccessEventAction { get; set; } = "Register Success";
        public string RegisterFailEventAction { get; set; } = "Register Fail";
        public string RegisterSubmitEventAction { get; set; } = "Register Submit";

        /// <summary>
        /// for some reason the TempData is lost if we set it in the callback after a redirect back from the social auth provider
        /// and the tracking doesn't appear in rendered markup so making this true
        /// but making it an option in case it can be resolved later
        /// as I don't know why it isn't working
        /// </summary>
        public bool TrackSocialLoginServerSide { get; set; } = true;

    }
}
