syntax on
set makeprg=g++\ % "" make settings
set number	"" show number left
set smartindent		"" smart indent
set autoread
set cin
"" Search Settings Begin
set is	"" use sequence search
set ic	"" ignore case
set hlsearch
"" Search Settings end

set textwidth=80
set colorcolumn=80
highlight ColorColumn ctermbg=darkgray

set backspace=indent,eol,start
set backspace=2
set tabstop=2
set softtabstop=2
set shiftwidth=2
set expandtab

abbr inc include

set wildmode=longest:list,full
" Key Binding
nmap <F4> :q! <CR>
noremap <F5> :!g++ -std=c++14 % -o output && echo "Succesfully Compiled" &&  ./output <CR>
noremap <F2> :!echo "Runned" && ./output <CR>
nmap <F3> :w <CR>
"ULTTISNIPS
let g:UltiSnipsExpandTrigger="<tab>"
" highlight
let g:cpp_member_variable_highlight = 1
let g:cpp_class_scope_highlight = 1
let g:cpp_class_decl_highlight = 1
let g:cpp_experimental_simple_template_highlight = 1
let g:cpp_experimental_template_highlight = 1
let g:cpp_concepts_highlight = 1
let g:cpp_no_function_highlight = 1

