## cloudscribe SimpleContent

Notes to myself - capturing my thoughts 2016-08-02

there are 2 "project" concepts that are kind of overlayed on top of each other

there is the "Content Project" with settings encapsulated in ProjectSettings class

there is also the concept in NoDb of a projectid which corresponds to the folder where obejcts are stored on disk
in some cases the same projectid may be used for both NoDb and ProjectSettings.ProjectId

in SimpleContent the ProjectSettings.ProjectId always corresponds to the NoDb projectid aka folder where
posts and pages will be stored

the current NoDb implementation for cloudscribe Core puts everything in a NoDb projectid/folder named "default"
so the NoDb projectid for cloudscribe core is not the same one used for cloudscribe SimpleContent

when using cloudscribe core we also have the concept of an AliasId on the site which is a friendlier id than
Id which is a guid

with integration between cloudscribe Core and SimpleContent, either the Site.AliasId or the Site.Id guid will be used as the 
ProjectSettings.ProjectId
if a project

I am kind of torn on these ideas, flexible but perhaps error prone if AliasId were to change for example
ProjectSettings object will get serialized to the cloudscribe core NoDb project ie "default"
but SimpleContent Pages and Posts will ALWAYS use ProjectSettings.ProjectId as the NoDb projectid
which means they will not be stored in the same NoDb project folder as cloudscribe Core data (ie default)

I had thought about making a NoDb ProjectIdResolver for cloudscribe Core that would enable each site/tenant to have its
own NoDb project whereas currently all Sites and related data would go into the "default" project
however for this to work we would have to resolve the NoDb projectId before resolving the site
in order to be able to look in the correct folder location to resolve the site
so we would need some extra master list of sites perhaps in the default NoDb project
common data such as Country/State list data would also still live in the default project
the advantages of this approach are easy to copy/migrate a specific site/tenant data from one install to another
and fewer files to iterate over when looking up info for a specific tenant
changing to this approach would also allow unification of the same NoDb projectid for SimpleContent as is used for the 
site specific cloudscribe Core data
typing this info out has helped me reason about this and I think I will go back nnow and try to implement this in 
the cloudscribe Core implementation of NoDb


## cloudscribe Core 

the goal is to be able to use a different NoDb projectid per site
so that all site related data can be segragated on disk into its own proejct folder
there are several considerations to address for this to work

the chicken or the egg problem, to lookup a site which is stored in NoDb we have to know the projectid where it is stored
and since we want to determine the NoDb projectid based on the site we have a circular lookup problem, 
we need to resolve the projectid in order to lookup the site in order to lookup the projectid

I thought about having a central list that maps host names folder etc but how to keep that list maintained is tricky
perhaps a better solution is changing how we lookup sites both individually and as a list 
so that we look for SiteSettings data in all project folders, it will require custom NoDb query logic for sites and site hosts
but that would be more straight forward and less error prone than having some hidden list that we try to update from code
if new sites are created from the UI

Note that ONLY SiteSettings would need to be looked up this way, once we have SiteSettings we have Site.Id as the projectid to use for all other data
so this class can be simple, we just inject current SiteSettings and use Id.ToString() as the projectid

but there is perhaps an additional issue for initial site creation of the first site, since the users and roles are created immediately
after the site is created it will not be resolved into the context so this resolver will not work for the commands that will create the user and roles
maybe we don't need the resolver at all and can always just use the site.Id as the projectid

another consideration to be dealt with is CoreData ie country/state list info which is common across tenants
to handle that we will make it always use DefaultProjectResolver so that data will always be stored in the default NoDb project