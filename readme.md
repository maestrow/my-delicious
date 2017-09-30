Save my [Delicious](https://del.icio.us) bookmarks. 

## Up & Run

- [Bootstrap Paket](https://gist.github.com/maestrow/94d99017380adbcadff29f048f423729#file-paket-bootstrap-md)
- `paket generate-load-scripts --framework netstandard2.0 --framework net40`
- Use scripts: `download.fsx`, `extract.fsx`

## How it works

`download.fsx` downloads all pages from your del.icio.us account into `contents` folder. `extract.fsx` extracts all links data from all html files in `content` folder and saves it as `data.xml`.

## ToDo

- Release importing script to import `data.xml` to google.bookmarks.