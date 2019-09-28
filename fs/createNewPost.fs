open System
// name of new post
let title = "UI tailoryourrevit"
let now = DateTime.Now.ToString "yyyy-MM-dd-hhmmss"
let nameOfPost = sprintf "%s-%s.markdown" now (title.Split ' ' |> String.concat "-")
// 2019-09-05 05:25:19 +0200
let frontmatter tit dat cat tags = 
    sprintf """---
layout: post
title: "%s"
date: %s 
categories: %s
tags: %s
---
    """ tit dat cat tags

// create file of new post
let dirPost = sprintf @"%s/_posts" __SOURCE_DIRECTORY__
dirPost |> System.IO.Directory.SetCurrentDirectory
let pthPost = sprintf @"%s/%s" dirPost nameOfPost
let file = pthPost |> System.IO.File.Create
file.Close()
// write frontmatter
let content = 
    frontmatter title now "" "" 
    //|> fun str -> str.Split '\n'  
(pthPost, content) |> System.IO.File.WriteAllText