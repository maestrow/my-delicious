#load ".paket/load/netstandard2.0/HtmlAgilityPack.CssSelectors.NetCore.fsx"
#load "paket-files/maestrow/HtmlAgilityPack.FSharp/HtmlAgilityPack.FSharp.fs"
#load ".paket/load/net40/SharpXml.fsx"


open HtmlAgilityPack
open HtmlAgilityPack.CssSelectors.NetCore
open HtmlAgilityPack.FSharp
open SharpXml

open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions
open System.Xml.Linq

// Settings
let rootDir = __SOURCE_DIRECTORY__
let contentDir = rootDir + "/content"
let outputFile = rootDir + "/data.xml"

let getContent path =
  File.ReadAllText path

let getFileNameByPageNum = sprintf "page_%d.html"

let getFullPath filename = sprintf "%s/%s" contentDir filename

type Link = 
  {
    Url: string
    Title: string
    Brief: string
    SavedAt: DateTime
    Tags: string list
  }
let extractLinkNodes = querySelectorAll ".articleThumbBlockOuter" 

let createLink (node: HtmlNode) = 
  let getUrl      = querySelectorAll ".articleInfoPan p"  >> Seq.head >> querySelector "a" >> innerText
  let getTitle    = querySelector    ".articleTitlePan a" >> innerText
  let getBrief    = querySelectorAll ".thumbTBriefTxt p"  >> 
    function
    | s when s.Count = 0 -> String.Empty
    | s -> s.[1] |> innerText
  let getDateText = querySelectorAll ".articleInfoPan p"  >> Seq.item 2 >> innerText
  let getTags     = querySelectorAll ".thumbTBriefTxt ul.tagName li a" >> Seq.map innerText
  let getDate txt = 
    let rx = Regex " on (.+)$" // sample: This link recently saved by shmelev on September 30, 2010
    let m = rx.Match txt
    m.Groups.[1].Value |> DateTime.Parse
  let nodes = [node]
  {
    Url     = nodes |> getUrl
    Title   = node  |> getTitle 
    Brief   = nodes |> getBrief 
    SavedAt = nodes |> getDateText |> getDate
    Tags    = nodes |> getTags     |> List.ofSeq
  }

let getLinkNodesForPage : int -> IList<HtmlNode> = 
  getFileNameByPageNum
  >> getFullPath
  >> getContent
  >> createDoc
  >> fun doc -> [doc]
  >> extractLinkNodes

let formatXml xml = 
  XDocument.Parse xml 
  |> fun i -> i.ToString ()

let saveTo file content = 
  File.WriteAllText (file, content)

let links = 
  [1..104]
  |> List.map getLinkNodesForPage
  |> Seq.concat
  |> Seq.map createLink 
  |> List.ofSeq

links 
|> List.ofSeq
|> XmlSerializer.SerializeToString
|> formatXml
|> saveTo outputFile