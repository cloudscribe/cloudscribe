
https://tutorialzine.com/2018/03/3-amazing-bootstrap-4-gallery-templates

https://github.com/feimosi/baguetteBox.js

How to resize, export & compress images for optimal website performance
https://www.foregroundweb.com/image-size/
* use double size thumbnails to look good on retina displays

* use 70-80 quality when compressing jpg. Only when you start to go lower than 50-60% do you start getting discernible image noise. But between 100 and 70-80, the quality difference is imperceptible, while the difference in image file size is huge.

* For full-width slideshows (that automatically stretch to the full size of the browser), I recommend going for 2560 pixels in width, which is the common resolution width for 27″ and 30″ monitors.

* Whenever an image needs to enlarge on screen (like in a lightbox display after clicking on a thumbnail, possibly part of a gallery slideshow), keep them to a max of 1500 pixels in width (and a max height of around 800-900 pixels), that’s usually acceptable.

* When using the <img> tag to adjust image size, only specify either the width or height attribute, not both.  This allows you to keep the same ratio.  Specifiying both width and height attributes could distort the image being displayed.

* Optimal size for bootstrap carousel: https://stackoverflow.com/questions/25554020/bootstrap-carousel-with-photos-optimal-image-size a good idea to use an image that is at least 1024 px wide (possibly wider), 


https://www.1dogwoof.com/10-tips-best-image-size-blog/
690 pixels wide

https://raygun.com/blog/optimal-image-size-for-your-website/

## srcset

There is no optimal size for images, it depends entirely on your page design.
At this point in time, I would go for an image with a width of 1920 px for full screen desktop applications and decreasing the size for handhelds.

https://css-tricks.com/responsive-images-youre-just-changing-resolutions-use-srcset/

## CSS 3 object fit

https://caniuse.com/#feat=object-fit
2018-07-15 - works in edge but only for images, fully supported in other modern browsers
not suported in IE

https://www.creativebloq.com/css3/control-image-aspect-ratios-css3-2122968

* object-fit:contain; will cause the content (eg the image) to be resized so that it’s fully displayed with its intrinsic aspect ratio preserved, but it will still fit inside the dimensions set for the element.
* fill: This setting causes the element’s content to expand to completely fill the dimensions set for it, even if this does break its intrinsic aspect ratio.
* cover: Using this setting preserves the intrinsic aspect ratio of the element content, but alters the width and height so that the content completely covers the element. The smaller of the width and height is made to fit the element exactly, and the larger dimension overflows the element.

img {
height: 100px;
width: 100px;
object-fit: contain;
}

Object-position works in exactly the same way as background-position does for background images, and can take the same values (pixels, ems, percentages, keywords, etc). It specifies the position of a replaced element’s content inside the area of that element. For example:

img {
height: 100px;
width: 100px;
object-fit: contain;
object-position: top 75%;
}



## Aspect ratio calculator

https://andrew.hedges.name/experiments/aspect_ratio/