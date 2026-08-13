/**
 * ShadcnBlazor - Lightweight JavaScript Interop
 */

window.shadcnBlazor = {
  clickOutsideHandlers: new Map(),

  getSystemDarkMode: function () {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
  },

  getThemeState: function () {
    try {
      const raw = localStorage.getItem('shadcn-blazor-theme');
      if (raw) {
        const state = JSON.parse(raw);
        state.isSystemDark = this.getSystemDarkMode();
        return state;
      }
    } catch (e) {
      console.warn('ShadcnBlazor: Failed to read theme from localStorage', e);
    }
    return {
      mode: 'system',
      preset: 'zinc',
      radius: 0.5,
      isSystemDark: this.getSystemDarkMode()
    };
  },

  applyTheme: function (config) {
    const root = document.documentElement;

    // Apply dark class
    if (config.isDark) {
      root.classList.add('dark');
      root.setAttribute('data-theme', 'dark');
    } else {
      root.classList.remove('dark');
      root.setAttribute('data-theme', 'light');
    }

    // Apply color preset
    if (config.preset) {
      root.setAttribute('data-preset', config.preset);
    }

    // Apply radius
    if (typeof config.radius === 'number') {
      root.style.setProperty('--radius', `${config.radius}rem`);
    }

    // Persist to localStorage
    try {
      localStorage.setItem('shadcn-blazor-theme', JSON.stringify({
        mode: config.mode,
        preset: config.preset,
        radius: config.radius
      }));
    } catch (e) {
      console.warn('ShadcnBlazor: Failed to persist theme', e);
    }
  },

  registerClickOutside: function (elementId, dotNetRef, methodName = 'OnClickOutside') {
    this.unregisterClickOutside(elementId);

    const handler = function (event) {
      const el = document.getElementById(elementId);
      if (el && !el.contains(event.target)) {
        dotNetRef.invokeMethodAsync(methodName);
      }
    };

    const escHandler = function (event) {
      if (event.key === 'Escape') {
        dotNetRef.invokeMethodAsync(methodName);
      }
    };

    document.addEventListener('mousedown', handler);
    document.addEventListener('touchstart', handler);
    document.addEventListener('keydown', escHandler);

    this.clickOutsideHandlers.set(elementId, { handler, escHandler });
  },

  unregisterClickOutside: function (elementId) {
    if (this.clickOutsideHandlers.has(elementId)) {
      const { handler, escHandler } = this.clickOutsideHandlers.get(elementId);
      document.removeEventListener('mousedown', handler);
      document.removeEventListener('touchstart', handler);
      document.removeEventListener('keydown', escHandler);
      this.clickOutsideHandlers.delete(elementId);
    }
  },

  positionFloating: function (triggerId, floatingId, placement = 'bottom-start', offset = 4) {
    const trigger = document.getElementById(triggerId);
    const floating = document.getElementById(floatingId);
    if (!trigger || !floating) return;

    const rect = trigger.getBoundingClientRect();
    const floatingRect = floating.getBoundingClientRect();
    const scrollX = window.scrollX || window.pageXOffset;
    const scrollY = window.scrollY || window.pageYOffset;

    let top = 0;
    let left = 0;

    switch (placement) {
      case 'bottom-start':
      case 'bottom':
        top = rect.bottom + offset + scrollY;
        left = rect.left + scrollX;
        break;
      case 'bottom-end':
        top = rect.bottom + offset + scrollY;
        left = rect.right - floatingRect.width + scrollX;
        break;
      case 'top-start':
      case 'top':
        top = rect.top - floatingRect.height - offset + scrollY;
        left = rect.left + scrollX;
        break;
      case 'top-end':
        top = rect.top - floatingRect.height - offset + scrollY;
        left = rect.right - floatingRect.width + scrollX;
        break;
      case 'right':
        top = rect.top + (rect.height - floatingRect.height) / 2 + scrollY;
        left = rect.right + offset + scrollX;
        break;
      case 'left':
        top = rect.top + (rect.height - floatingRect.height) / 2 + scrollY;
        left = rect.left - floatingRect.width - offset + scrollX;
        break;
    }

    // Viewport bounding clamp
    const padding = 8;
    const maxLeft = window.innerWidth - floatingRect.width - padding;
    left = Math.max(padding, Math.min(left, maxLeft));

    floating.style.position = 'absolute';
    floating.style.top = `${top}px`;
    floating.style.left = `${left}px`;
  },

  copyToClipboard: async function (text) {
    try {
      await navigator.clipboard.writeText(text);
      return true;
    } catch (e) {
      console.error('ShadcnBlazor: Failed to copy to clipboard', e);
      return false;
    }
  },

  lockScroll: function () {
    document.body.style.overflow = 'hidden';
  },

  unlockScroll: function () {
    document.body.style.overflow = '';
  }
};

// Listen for system color scheme changes
if (window.matchMedia) {
  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function (e) {
    const themeState = window.shadcnBlazor.getThemeState();
    if (themeState.mode === 'system') {
      themeState.isDark = e.matches;
      window.shadcnBlazor.applyTheme(themeState);
    }
  });
}
