// Downloads all pages from your del.icio.us account.

open System.Net
open System
open System.IO

// Settings
let rootDir = __SOURCE_DIRECTORY__
let saveDir = rootDir + "/content"
let urlTpl = sprintf "https://del.icio.us/shmelev?&page=%d"


let save dir pageNum content = 
  let path = sprintf "%s/page_%d.html" dir pageNum
  File.WriteAllText (path, content)
  
let fetchUrlAsync url =        
  async {
    let req = WebRequest.Create(Uri(url)) 
    use! resp = req.AsyncGetResponse() 
    use stream = resp.GetResponseStream() 
    use reader = new IO.StreamReader(stream) 
    return reader.ReadToEnd() 
  }

let savePage pageNum = 
  async {
    let! content = 
      urlTpl pageNum
      |> fetchUrlAsync
    save saveDir pageNum content
    printfn "got #%d" pageNum
  }


#time                        // turn interactive timer on
let res = 
  [1..104]
  |> List.map savePage       // make a list of async tasks
  |> Async.Parallel          // set up the tasks to run in parallel
  |> Async.RunSynchronously  // start them off
#time                        // turn timer off