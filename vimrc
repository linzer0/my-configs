set nocompatible              " be iMproved, required
set hls 
set number "This command show numbers from left side
set autoindent
set autoread "When we changed some file, he will update without question about it
syntax on
filetype on                  " required

"Languages config
augroup lan_configs
	autocmd FileType cpp source /etc/vim/configs/cpp.vim
	autocmd FileType python source /etc/vim/configs/python.vim
	autocmd FileType javascript source /etc/vim/configs/javascript.vim
augroup END	

" Key bindings
nmap <F4> :q!<CR>
nmap <F3> :w <CR>
nmap <F6> :set paste<cr>
nmap <F7> :set nopaste<cr>
map <c-h> :tabprevious<CR>
map <c-l> :tabnext<CR>


set backspace=indent,eol,start
set backspace=2
set wildmode=longest:list,full

set rtp+=~/.vim/bundle/Vundle.vim
call vundle#begin()
	Plugin 'bling/vim-airline'

	Plugin 'google/vim-maktaba'
	Plugin 'google/vim-codefmt'
	Plugin 'google/vim-glaive'

	Plugin 'VundleVim/Vundle.vim'
call vundle#end()       

augroup autoformat_settings
  autocmd FileType c,cpp,proto,javascript AutoFormatBuffer clang-format
  autocmd FileType html,css,json AutoFormatBuffer js-beautify
  autocmd FileType python AutoFormatBuffer yapf
  " Alternative: autocmd FileType python AutoFormatBuffer autopep8
augroup END

filetype plugin indent on
