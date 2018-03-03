# Message Formatting

https://www.grahamcluley.com/can-no-longer-recommend-mailchimp/

Free online email designer
https://topol.io/

https://www.campaignmonitor.com/blog/email-marketing/2017/05/the-really-good-guide-to-email-design-bonus-checklist/

https://www.smashingmagazine.com/2017/01/making-responsive-html-email-coding-easy-with-mjml/

https://www.smashingmagazine.com/2017/01/introduction-building-sending-html-email-for-web-developers/

https://www.smashingmagazine.com/2014/08/improve-your-email-workflow-with-modular-design/

https://litmus.com/blog/the-ultimate-guide-to-using-snippets-in-email-design
https://litmus.com/community/snippets

https://litmus.com/community/templates

https://github.com/leemunroe/responsive-html-email-template

https://htmlemail.io/

http://foundation.zurb.com/emails.html

https://developers.google.com/gmail/markup/actions/actions-overview

https://github.com/fcarneiro/tedc15_template

http://mehdi.me/generating-html-emails-with-razorengine-introduction/
https://github.com/mehdime/RazorEngineEmails/tree/master/src/Part%2003/ConsoleApplication




Responsive Email Made Easy
https://mjml.io/

https://mosaico.io/
https://github.com/voidlabs/mosaico


http://www.vsysad.com/2014/09/setup-and-configure-smtp-server-on-windows-server-2012/

http://www.enom.com/kb/kb/kb_0394-reverse-dns-ptr.htm

SMTP Strict Transport Security
https://tools.ietf.org/html/draft-margolis-smtp-sts-00

http://blog.guessbox.io/the-persuasive-power-of-professional-email-signatures/

# Sending Messages

## SendGrid

https://sendgrid.com/docs/API_Reference/index.html

https://sendgrid.com/partners/contact-us-reseller/

NetStandard
https://github.com/sendgrid/sendgrid-csharp

Request
POST https://api.sendgrid.com/v3/mail/send HTTP/1.1

curl -X "POST" "https://api.sendgrid.com/v3/mail/send" -H "Authorization: Bearer YOUR_API_KEY" -H "Content-Type: application/json" -d "[YOUR DATA HERE]"


Request Body
{
  "personalizations": [
    {
      "to": [
        {
          "email": "john@example.com"
        }
      ],
      "subject": "Hello, World!"
    }
  ],
  "from": {
    "email": "from_address@example.com"
  },
  "content": [
    {
      "type": "text/plain",
      "value": "Hello, World!"
    }
  ]
}

Response
{
  HTTP/1.1 202
}


## MailGun
https://documentation.mailgun.com/en/latest/api_reference.html


## Elastic Email

https://elasticemail.com/api-documentation/

Most of API requests should be sent using an HTTP GET method. If a method needs sending using an HTTP POST method, it is designated in the method description.

Use	Full Path for API connection
Base URL	Path	Parameters
Elastic Email	https://api.elasticemail.com	/v2/category/action	?param1=value1&param2=value2
Private Branding*	https://api.yourdomain.com
Example	https://api.elasticemail.com	/v2/contact/list	?apikey=your-apikey
* You can create a CNAME in your DNS software and turn on private branding on your Account screen to brand api calls for your customers

https://api.elasticemail.com/public/help#Email_Send

Returns (if successful)
{"success": true, "data": { EmailSend } }
Returns (if failed)
{"success": false, "error": string with error message}

https://api.elasticemail.com/v2/email/send?
apikey=94DAF66E-4DF6-4E8E-AF96-D094A8D21DF3
&subject=
&from=
&fromName=
&sender=
&senderName=
&msgFrom=
&msgFromName=
&replyTo=
&replyToName=
&to=
&msgTo=
&msgCC=
&msgBcc=
&lists=
&segments=
&mergeSourceFilename=
&channel=
&bodyHtml=
&bodyText=
&charset=
&charsetBodyHtml=
&charsetBodyText=
&encodingType=
&template=
&headers_firstname=firstname: myValueHere
&postBack=
&merge_firstname=John
&timeOffSetMinutes=
&poolName=My Custom Pool
&isTransactional=false

Name	                       Type	  Required	  Default	 Description
apikey	                       string	Yes		             ApiKey that gives you access to our SMTP and HTTP API's.
bodyHtml	                   string	No	       null	     Html email body
bodyText	                   string	No	       null	     Text email body
channel	                       string	No	       null	     An ID field (max 191 chars) that can be used for reporting [will default to HTTP API or SMTP API]
charset	                       string	No	       null	     Text value of charset encoding for example: iso-8859-1, windows-1251, utf-8, us-ascii, windows-1250 and more…
charsetBodyHtml	               string	No	       null	     Sets charset for body html MIME part (overrides default value from charset parameter)
charsetBodyText	               string	No	       null	     Sets charset for body text MIME part (overrides default value from charset parameter)

encodingType	               EncodingType	No	ApiTypes.EncodingType.None	0 for None, 1 for Raw7Bit, 2 for Raw8Bit, 3 for QuotedPrintable, 4 for Base64 (Default), 5 for Uue note that you can also provide the text version such as "Raw7Bit" for value 1. NOTE: Base64 or QuotedPrintable is recommended if you are validating your domain(s) with DKIM.

from	                       string	No	       null	     From email address
fromName	                   string	No	       null	     Display name for from email address

headers	Repeated list of string keys and string values	No	null	Optional Custom Headers. Request parameters prefixed by headers_ like headers_customheader1, headers_customheader2. Note: a space is required after the colon before the custom header value. headers_xmailer=xmailer: header-value1

isTransactional	              boolean	No	       false	  True, if email is transactional (non-bulk, non-marketing, non-commercial). Otherwise, false

lists	List of string	No	null	The name of a contact list you would like to send to. Separate multiple contact lists by commas or semicolons.

merge	Repeated list of string keys and string values	No	null	Request parameters prefixed by merge_ like merge_firstname, merge_lastname. If sending to a template you can send merge_ fields to merge data with the template. Template fields are entered with {firstname}, {lastname} etc.

mergeSourceFilename	            string	No	       null	     File name one of attachments which is a CSV list of Recipients.

msgBcc	List of string	No	null	Optional parameter. Will be ignored if the 'to' parameter is also provided. List of email recipients (each email is treated seperately). Separated by comma or semicolon.

msgCC	List of string	No	null	Optional parameter. Will be ignored if the 'to' parameter is also provided. List of email recipients (visible to all other recipients of the message as CC MIME header). Separated by comma or semicolon.

msgFrom	string	No	null	Optional parameter. Sets FROM MIME header.
msgFromName	string	No	null	Optional parameter. Sets FROM name of MIME header.
msgTo	List of string	No	null	Optional parameter. Will be ignored if the 'to' parameter is also provided. List of email recipients (visible to all other recipients of the message as TO MIME header). Separated by comma or semicolon.
poolName	string	No	null	Name of your custom IP Pool to be used in the sending process
postBack	string	No	null	Optional header returned in notifications.
replyTo	string	No	null	Email address to reply to
replyToName	string	No	null	Display name of the reply to address
segments	List of string	No	null	The name of a segment you would like to send to. Separate multiple segments by comma or semicolon. Insert "0" for all Active contacts.
sender	string	No	null	Email address of the sender
senderName	string	No	null	Display name sender
subject	string	No	null	Email subject
template	string	No	null	The ID of an email template you have created in your account.
timeOffSetMinutes	string	No	null	Number of minutes in the future this email should be sent up to a maximum of 1 year (524160 minutes)
to	List of string	No	null	List of email recipients (each email is treated separately, like a BCC). Separated by comma or semicolon. We suggest using the "msgTo" parameter if backward compatibility with API version 1 is not a must.
Attach the file as POST multipart/form-data file upload
