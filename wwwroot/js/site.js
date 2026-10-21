// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const appShell = document.querySelector('.app-shell');
const sidebarToggleButton = document.querySelector('#sidebar-toggle');
const sidebarOverlay = document.querySelector('#sidebar-overlay');

function setSidebarState(isOpen) {
  const isMobile = window.matchMedia('(max-width: 767.98px)').matches;

  appShell.classList.toggle('sidebar-open', isMobile && isOpen);
  appShell.classList.toggle('sidebar-hidden', !isMobile && !isOpen);
  sidebarToggleButton?.setAttribute('aria-expanded', isOpen.toString());
  sidebarToggleButton?.setAttribute(
    'title',
    isOpen ? 'Hide navigation' : 'Show navigation',
  );
  sidebarToggleButton?.setAttribute(
    'aria-label',
    isOpen ? 'Hide navigation' : 'Show navigation',
  );
}

sidebarToggleButton?.addEventListener('click', () => {
  const isMobile = window.matchMedia('(max-width: 767.98px)').matches;
  const isOpen = isMobile
    ? appShell.classList.contains('sidebar-open')
    : !appShell.classList.contains('sidebar-hidden');

  setSidebarState(!isOpen);
});
sidebarOverlay?.addEventListener('click', () => setSidebarState(false));

window.addEventListener('resize', () => {
  const isMobile = window.matchMedia('(max-width: 767.98px)').matches;
  setSidebarState(
    isMobile
      ? appShell.classList.contains('sidebar-open')
      : !appShell.classList.contains('sidebar-hidden'),
  );
});

setSidebarState(!window.matchMedia('(max-width: 767.98px)').matches);
