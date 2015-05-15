# Modal Dialogs

seems like it is hard to do modal dialog in a generic way, we have to be strongly tied to a js framework bootstrap or jqueryui
in cloudscribe we have implemented both bootstrap and jqueryui modal dialog implementations, but each expects different structure 
and css classes in the partial view, where they want the headings to be is different in each case.
We opted for bootstrap, so if you want to change to the jqueryui version you would need to override the needed views and change the markup.

other gotchas
seems normally a partial view is used for a modal
but this poses problems for paging content within the modal
paging inside the dialog should/would work naturally if the dialog uses an iframe
http://stackoverflow.com/questions/5660263/how-to-display-an-iframe-inside-a-jquery-ui-dialog
-- chosen solution below is to use ajax to do paging within the open modal without need for any iframe

**cloudscribe implementation reference** 
in role management there is a modal for finding users in a role or users not in a role.
I used ajax for the paging within the modal there, so no iframe was needed.
If there is a need  to implement that kind of thing elsewhere it is a good example.
See RoleMembers.cshtml which opens the modal when you click the "Add User" button.
The modal allows searching and paging through users.
On the first opening of the dialog the view NonMembersPartial.cshtml is loaded as a partial view.
Inside that partial we load NonMembersGridPartial.cshtml
Subsequent updates by user searching or paging are done from inside this view using ajax.
No iframes were involved in the implementation.


**other notes about challenges and research into solutions for modal dialogs**

specific problems of nested modal dialogs
in the country list for example we provide a link to edit the country which is done in a modal, when the modal is dismissed the underlying page is refreshed so that any change in the data will be shown. This works well, since it is only one modal at a time.
In the same contry list page we show a link for the states of that country, which also opens in a modal, the modal is pageable as well since there can be many states.
So the state list modal is already open and now we want to edit a state without losing our page in the statelist modal or the underlying page with country list. So we make the link to edit a state also a modal, resulting in a modal on top of a modal.
-- problem -- 
after editing a state, when the upper modal is dismissed, how do we refresh the content in the state list modal without losing the page. we don't really have any reference to the statelist modal from inside the state edit modal, and we don't have any easy way to pass a reference in there.

-- possible strategies --
we could try to use just one modal and replace the contents using ajax, so the same modal could be used for the state list and state edit, with some link to retore the list after the edit. sort of like SPA (single page application) within the modal

or if we can find a way to refresh the parent modal upon dismissing the child

**Some usefull posts and articles from research**

http://getbootstrap.com/javascript/#modals

http://nakupanda.github.io/bootstrap3-dialog/

http://stackoverflow.com/questions/19305821/bootstrap-3-0-multiple-modals-overlay

http://miles-by-motorcycle.com/fv-b-8-670/stacking-bootstrap-dialogs-using-event-callbacks

http://miles-by-motorcycle.com/fv-b-8-670/stacking-bootstrap-dialogs-using-event-callbacks

http://www.whiletrue.it/how-to-update-the-content-of-a-modal-in-twitter-bootstrap/

http://stackoverflow.com/questions/12449890/reload-content-in-modal-twitter-bootstrap

confirming deletes and other actions

http://rkandhal.com/07/07/reusable-modal-dialogs/

http://jameschambers.com/2014/07/day-29-confirmation-dialogs-for-delete-actions/

http://www.codeguru.com/csharp/.net/net_asp/mvc/article.php/c20139/Confirming-Delete-Operations-in-ASPNET-MVC.htm

http://rohit-developer.blogspot.in/2014/08/crud-multiple-jquery-dialogs-in-aspnet.html

http://stackoverflow.com/questions/4682107/delete-actionlink-with-confirm-dialog

https://github.com/miyabis/jquery.iframeDialog

http://stackoverflow.com/questions/512257/jquery-modal-boxes-and-iframe?rq=1

http://www.codeproject.com/Tips/826002/Bootstrap-Modal-Dialog-Loading-Content-from-MVC-Pa
http://www.devzest.com/blog/post/ASPNet-MVC-Modal-Dialog.aspx

http://www.codeproject.com/Articles/146396/A-Comparison-of-Three-jQuery-Modal-Dialogs-for-ASP

http://code.daypilot.org/81367/daypilot-modal

http://www.asp.net/mvc/overview/older-versions-1/views/creating-custom-html-helpers-cs