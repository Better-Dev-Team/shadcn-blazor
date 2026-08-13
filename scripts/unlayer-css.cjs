const fs = require('fs');

const file = process.argv[2];
if (!file) {
  console.error('Usage: node unlayer-css.cjs <compiled.css>');
  process.exit(1);
}

let css = fs.readFileSync(file, 'utf8');

css = css.replace(/@layer\s+theme\s*,\s*base\s*,\s*components\s*,\s*utilities\s*;/, '');
css = css.replace(/@layer\s+(?:theme|base|components|utilities|properties)\s*;/g, '');

for (const layerName of ['theme', 'base', 'components', 'utilities', 'properties']) {
  const re = new RegExp('@layer\\s+' + layerName + '\\s*\\{');
  let match;
  while ((match = re.exec(css)) !== null) {
    const openIdx = match.index + match[0].lastIndexOf('{');
    const closeIdx = findMatchingBrace(css, openIdx);
    css = css.slice(0, match.index) + css.slice(openIdx + 1, closeIdx) + css.slice(closeIdx + 1);
  }
}

fs.writeFileSync(file, css);
console.log('Unwrapped cascade layers in ' + file);

function findMatchingBrace(s, openIdx) {
  let depth = 0;
  let quote = null;
  for (let i = openIdx; i < s.length; i++) {
    const ch = s[i];
    if (quote) {
      if (ch === quote && s[i - 1] !== '\\') quote = null;
      continue;
    }
    if (ch === '"' || ch === "'") {
      quote = ch;
      continue;
    }
    if (ch === '{') depth++;
    else if (ch === '}') {
      depth--;
      if (depth === 0) return i;
    }
  }
  return s.length - 1;
}
