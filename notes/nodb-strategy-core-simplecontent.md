## cloudscribe SimpleContent

Notes to myself - capturing my thoughts 2016-08-02

there are 2 "project" concepts that are kind of overlayed on top of each other

there is the "Content Project" with settings encapsulated in ProjectSettings class

there is also the concept in NoDb of a projectid which corresponds to the folder where obejcts are stored on disk
in some cases the same projectid may be used for both NoDb and ProjectSettings.ProjectId

in SimpleContent the ProjectSettings.ProjectId always corresponds to the NoDb projectid aka folder where
posts and pages will be stored

I decided for cloudscribe Core to enable each site/tenant to have its
own NoDb project whereas common data such as country/state/logging data would go into the "default" project

the advantages of this approach are easy to copy/migrate a specific site/tenant data from one install to another
and fewer files to iterate over when looking up info for a specific tenant
changing to this approach would also allow unification of the same NoDb projectid for SimpleContent as is used for the 
site specific cloudscribe Core data

Specifically this means using the Site.Id as the NoDb projectId, so that once we have resolved the site we already 
have the needed NoDb project id


## cloudscribe Core 

the goal is to be able to use a different NoDb projectid per site
so that all site related data can be segragated on disk into its own proejct folder
there are several considerations to address for this to work

the chicken or the egg problem, to lookup a site which is stored in NoDb we have to know the projectid where it is stored
and since we want to determine the NoDb projectid based on the site we have a circular lookup problem, 
we need to resolve the projectid in order to lookup the site in order to lookup the projectid

a better solution is changing how we lookup sites both individually and as a list 
so that we look for SiteSettings data in all project folders, it will require custom NoDb query logic for sites and site hosts
but that would be more straight forward and less error prone than having some hidden list that we try to update from code
if new sites are created from the UI

Note that ONLY SiteSettings and SiteHosts need to be looked up this way, once we have SiteSettings we have Site.Id as the projectid to use for all other data
so this class can be simple, we just inject current SiteSettings and use Id.ToString() as the projectid

another consideration to be dealt with is CoreData ie country/state list info which is common across tenants
to handle that we will make it always use DefaultProjectResolver so that data will always be stored in the default NoDb project
