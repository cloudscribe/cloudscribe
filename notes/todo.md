TODO:

## Implement a way to plugin event handlers that can execute on various actions of interest

** user created, user login, user logout, user updated, pre-update, pre-delete, user deleted, added to role, removed from role, password changed, email changed
** site created, updated, pre-update, deleted, pre-deleted
** role created, updated deleted
These can be injected an a dependency can be taken on IEnumerable<ISomeHandler> to fire them when the action occurs

Implement logging with NoDb storage

Implement a way to use NoDb with project per tenant instead of all tenants in the same project



# Backlog Plans and ideas

Support for IdentityServer4 - main goal is I should be able to mix and mingle SPA features into the same web app with MVC features, and jwt auth works in addition to cookie auth

If the Registration Agreement is populated, if the user registers with social auth this is not currently enforced, the user can register without having checked the box. Need to detect new user creation for social auth and if needed redirect to another page to require the user to agree to the terms. but to do this would require a new field on user account to indicate if he agreed - update, if the user has to confirm their email
on the first social auth ie registering on the site we do have an opportunity there to also require them to agree so I implemented that.
But I think some social auth providers may provide a pre confirmed email and may bypass that screen - not 100% sure on this

Support for security questions and answers?

Support for custom account/registration fields?

Telemetry - I would like to implement telemetry to capture activities, but I want an abstraction of my own as opposed to being strongly coupled to Application Insights
I want to be able to log activities to an activity stream - ie into a CRM - there should be interfaces for this in cloudscribe core, but implementation will be outside or can use Application Insights

Need some kind of help system to add contextual help for various settings that may need explanation

--
moved this to backlog, because it is a difficult thing to implement and maybe not needed
when I tried to enforce this I found that that user has no way to login and add a phone
so phone should maybe only be an optional 2 factor aquth source
for now removing the checkbox from the ui
Check "Require Confirmed Phone" && provide SMS settings
** Newly registered user should not be able to login until phone is verified by sms
** User should have an opportunity to provide a phone number during registration
--
