namespace cloudscribe.Core.Web.Design
{
    public class CoreIconSet
    {
        public string SetId { get; set; } = "glyphicons";

        public string Email { get; set; } = "glyphicon glyphicon-envelope";
        public string Username { get; set; } = "glyphicon glyphicon-circle-arrow-right";
        public string Password { get; set; } = "glyphicon glyphicon-lock";
        public string FirstName { get; set; } = "glyphicon glyphicon-user";
        public string LastName { get; set; } = "glyphicon glyphicon-user";
        public string DateOfBirth { get; set; } = "glyphicon glyphicon-calendar";

        // the glyphicons that come with bootstrap 3 don't have any social icons
        // so using font awesome but they just won't render anything unless using fontawesom
        public string Facebook { get; set; } = "fa fa-facebook-square";
        public string Google { get; set; } = "fa fa-google";
        public string Twitter { get; set; } = "fa fa-twitter";
        public string Microsoft { get; set; } = "fa fa-windows";
        public string OpenIDConnect { get; set; } = "fa fa-arrow-circle-right";


    }
}
