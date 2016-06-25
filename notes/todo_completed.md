Completed:

2016-06-15 Tested
Uncheck "Really Delete Users" - then delete a user
** He should still exist in the db, but does not appear in the user list, he is flagged as IsDeleted = true
** He should not be able to login 
** Currently only way to see a user flagged as deleted is dorectly in the db, not in the UI
** Changing back to checked on Really Delete Users does not delete existing rows flagged as deleted
-- end tested 2016-06-15

2016-06-16 Tested
Uncheck "Allow New Registrations"
** Register links should not be visible
** It should not be possible to register by visiting /Account/Register - returns 404

Uncheck "Allow Persistent Login"
** Remember me should not appear on login page
** Auth cookie should expire when the browser is closed

Uncheck "Use Email for Login"
** Login page label should show username as opposed to email
** should not be possible to login using email
** Register page should show input for username

-- end tested 2016-06-16

2016-06-19 tested
If the Registration Agreement is populated, the user must check the box agreeing to the terms of use in order to register

Check "Only Use Social Authentication" and configure social auth
** The only way for new users to register or for any user to login should be via social auth
** definitely the admin needs to add a social auth to his own account before making this setting or he will have no way to login without changing it in the db back to allow regular login
** I think we still want a visible "Register" link, but the Register action should redirect to login in this case

Check "Require Approval Before Login"
** Newly registered user should not be able to login until an admin approves the account
** This should be true even if using social auth to create the account
** An email should be sent to the notify list csv
** There should be a way for an admin to approve an account and generate an email to the new user letting him know of approval


Check "Require Confirmed Email" && provide smtp settings so email can be sent for verification
** Newly registered user should not be able to login until the email is verified 
** This should be true even if using social auth to create the account

-- end tested 2016-06-19

Replace jqueryui datepicker with bootstrap datetime picker
Redesign email templating for email to use Razor views 
Show registration agree ment on social auth confirm
Implement sorting links on user list