---
layout: post
title: "Learning Liquid"
date: 2019-09-12-063204 
categories: Web
tags: [liquid, learning, notes]
published: false
---
<!-- 
    Jekyll << Liquid << Ruby 
    Liquid: template language created by Shopify
-->
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

#### Liquid  
* *objects* use ``{% raw %} {{ }} {% endraw %}`` - This page has layout {{page.layout}}
* *tags* use ``{% raw %} {% if %} {% endif %} {% endraw %}`` {% if page.layout == "post" %} It is a {{ page.layout }} by {{ user.name }} {% endif %}  
* *filters* use ``|`` {{ "this string appends " | append: ".html" }}  

#### Tags
+ _control flow_  
    - *if* 
    - *unless*  
    - *elsif / else*  
    &emsp;``{% raw %} {% if %} {% elsif %} {% else %} {% endif %} {% endraw%}``
    - *case / when*
    &emsp;``{% raw %}{% assign a = b %} {% case a %} {% when b %} {% when c%} {% else %} {% endcase %}{% endraw %}``
+ _iteration_
    - for
        + *else* : if loop has _zero_ length
        + *break*
        + *continue* : skip the current iteration
    - for (parameters)
        + *limit* : {% raw %}{% ``for`` x in arr ``limit:2`` %}{% endraw %}
        + *offset* : {% raw %}{% ``for`` x in arr ``offset:2`` %}{% endraw %}
        + *range* : {% raw %}{% ``for`` x in arr ``(3..5)`` %}{% endraw %}
        + *reversed* : {% raw %}{% ``for`` x in arr ``reversed`` %}{% endraw %}  
    - *cycle* : loops through a group of _strings_
    - *cycle* (parameters) : multiple ``cycle`` blocks in one template
    - *tablerow* :
        {% raw %} ``<table>`` {% ``tablerow`` tag in page.tags %} {% endtablerow %}``</table>`` {% endraw %}  
    - tablerow (parameters)
        + *cols* : {% raw %}{% _tablerow_ tag in page.tags ``cols:2`` %} {% endtablerow %}{% endraw %}
        + *limit*
        + *offset*
        + *range* : {% raw %}{% _tablerow_ tag in page.tags ``(2..8)`` %} {% endtablerow %}{% endraw %}
+ *raw*
+ _variable_
    - *assign*
    - *capture* : {% raw %}{% ``capture`` var %} something {% endcapture %}{% endraw %}
    - *increment* : creates number variable, and increases value by 1 each time when called, starts at 0
    - *decrement* : decreases by 1, starts at -1
+ _filter_
    - *abs* : {% raw %}{{ -17 ``| abs`` }}}{% endraw %}  
    - *append* : {% raw %}{{"abc" ``| append:`` "DEF" }}{% endraw %}
    - *at_least* : {% raw %}{{ 4 ``| at_least:`` 5 }}{% endraw %} -> 5 (the min is 5)
    - *at_most* : {% raw %}{{ 4 ``| at_most:`` 3 }}{% endraw %} -> 3 (the max is 3)