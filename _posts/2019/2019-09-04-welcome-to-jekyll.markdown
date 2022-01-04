---
layout: post
title:  "Jekyll Test Page"
date:   2019-09-04 05:25:19 +0200
categories: Web
tags: [notes, jekyll]
h1: h1
h2: h2
h6: h6

published: false
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

You’ll find this post in your `_posts` directory. Go ahead and edit it and re-build the site to see your changes. You can rebuild the site in many different ways, but the most common way is to run `jekyll serve`, which launches a web server and auto-regenerates your site when a file is updated.

To add new posts, simply add a file in the `_posts` directory that follows the convention `YYYY-MM-DD-name-of-post.ext` and includes the necessary front matter. Take a look at the source for this post to get an idea about how it works.

## Liquid
# Objects
Show page title + categories: {{ page.title }} + {{ page.categories }}
<h1>{{ page.h1 }}</h1><h2>{{ page.h2 }}</h2><h6>{{ page.h6 }}</h6>
# Filters
How is the capitalized layout name? {{page.layout}} -> {{ page.layout | capitalize }}
# Tags
Tags are used for example for the code snippets ->

Jekyll also offers powerful support for code snippets:

{% highlight ruby %}
# This is a syntax highlight test of ruby
def print_hi(name)
  puts "Hi, #{name}"
end
print_hi('Tom')
#=> prints 'Hi, Tom' to STDOUT.
{% endhighlight %}

{% highlight fsharp %}
// This is a syntax highlight test of f#
#load "some.fs"
#IF INTERACTIVE
#r "other.dll"
#ENDIF
open Random.Namespace
let a, b = 10, 34
let sum = (a * b) |> Math.Func
printfn "%f is result" sum
{% endhighlight %}

{% highlight python %}
#This is a syntax highlight test of python
import System
a = 45
b = 32
a + b
{% endhighlight %}

**bold**, _italic_, `monospace` are like these.  
Inline <abbr title = "hyper text"> hypertext </abbr> is supported.

Check out the [Jekyll docs][jekyll-docs] for more info on how to get the most out of Jekyll. File all bugs/feature requests at [Jekyll’s GitHub repo][jekyll-gh]. If you have questions, you can ask them on [Jekyll Talk][jekyll-talk].

[jekyll-docs]: https://jekyllrb.com/docs/home
[jekyll-gh]:   https://github.com/jekyll/jekyll
[jekyll-talk]: https://talk.jekyllrb.com/
