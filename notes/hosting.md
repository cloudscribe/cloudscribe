
## matching host names

https://stackoverflow.com/questions/21173734/extracting-top-level-and-second-level-domain-from-a-url-using-regex
Match anything that isn't a dot, three times, from the end of the line using the $ anchor.

The last match from the end of the string should be optional to allow for .com.au or .co.nz type of domains.

Both the last and second last matches will only match 2-3 characters, so that it doesn't confuse it with a second-level domain name.

[^.]*\.[^.]{2,3}(?:\.[^.]{2,3})?$




https://serverfault.com/questions/595417/is-co-uk-a-top-level-domain



https://blog.linode.com/2017/02/14/high-memory-instances-and-5-linodes/

https://www.digitalocean.com/pricing/

https://joshtronic.com/2017/02/14/five-dollar-showdown-linode-vs-digitalocean-vs-lightsaild-vs-vultr/