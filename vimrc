set nocompatible              " be iMproved, required
set hls
set number
set autoindent
set autoread
syntax on
filetype on                  " required
augroup lan_configs
	autocmd FileType cpp source /home/linzet/configs/cpp.vim
	autocmd FileType python source /home/linzet/configs/python.vim
augroup END	
" Key bindings
nmap <F4> :q!<CR>
nmap <F3> :w <CR>
nmap <F6> :set paste<cr>
nmap <F7> :set nopaste<cr>
" A
map <c-h> :tabprevious<CR>
map <c-l> :tabnext<CR>
set backspace=indent,eol,start
set backspace=2
set wildmode=longest:list,full
set rtp+=~/.vim/bundle/Vundle.vim
call vundle#begin()
	Plugin 'MarcWeber/vim-addon-mw-utils'
	Plugin 'tomtom/tlib_vim'
	Plugin 'octol/vim-cpp-enhanced-highlight'
	Plugin 'nightsense/simplifysimplify' 
""	Plugin 'ervandew/supertab'
	Plugin 'bling/vim-airline'
	Plugin 'VundleVim/Vundle.vim'
call vundle#end()       
filetype plugin indent on
