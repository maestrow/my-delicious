Save my [Delicious](https://del.icio.us) bookmarks. 

## Up & Run

- [Bootstrap Paket](https://gist.github.com/maestrow/94d99017380adbcadff29f048f423729#file-paket-bootstrap-md)
- `paket generate-load-scripts --framework netstandard2.0 --framework net40`
- Use scripts: `download.fsx`, `extract.fsx`

Content:

- `download.fsx` - downloads all pages from your del.icio.us account. Next I'll extract all data (links, tags, captions) and export it in google.bookmarks.
- `content` - folder contains all downloaded content. I save it here for reliability as https://del.icio.us is unstable. 
- `extract.fsx` - extracts all links data from all html files in `content` folder and saves it as `data.xml`