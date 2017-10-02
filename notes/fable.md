## Fable enables you to write your client side code in F# and transpile it to javascript

http://fable.io/

https://github.com/kunjee17/awesome-fable

### Rationale

Javascript is not a great language, that is why people use TypeScript to make it a little better.
F# is far superior to using javascript/typescript
Now that major web browsers support webassembly, it is only a matter of time before new tools emerge to write client side code in better languages such as F# and C# and compile to webassembly. When that happens javascript will go out of style in favor of better languages.
My thinking is that by using Fable today we can write our client side code in F# and be able to re-use much of that code later but compiling to webassembly rather than transpiling to javascript. So I see it as a way to future proof client side code a bit and avoid investing a lot of time writing more javascript which will soon be legacy code.

### Resources for learning

This video is a good start
https://channel9.msdn.com/Events/dotnetConf/2017/T319

I have created a working spa sample for asp.net core here:
https://github.com/joeaudette/fable-samples/tree/master/SpaApp

https://github.com/fable-compiler


Fable applications following "model view update" architecture
https://fable-elmish.github.io/
https://github.com/fable-elmish

as an excercise I would like to conver this to F#
https://codepen.io/eltonkamami/pen/ECrKd