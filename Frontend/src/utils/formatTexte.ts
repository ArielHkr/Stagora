// Le modèle écrit parfois **en gras** (Markdown).
// On découpe le texte en morceaux plutôt que d'utiliser v-html :
// Vue échappe chaque morceau, donc aucun HTML venant de la réponse ne peut s'exécuter.

export interface Segment {
  texte: string
  gras: boolean
}

export function decouperGras(texte: string): Segment[] {
  const segments: Segment[] = []
  let position = 0

  for (const correspondance of texte.matchAll(/\*\*(.+?)\*\*/gs)) {
    const debut = correspondance.index ?? 0
    if (debut > position) {
      segments.push({ texte: texte.slice(position, debut), gras: false })
    }
    segments.push({ texte: correspondance[1] ?? '', gras: true })
    position = debut + correspondance[0].length
  }

  if (position < texte.length) {
    segments.push({ texte: texte.slice(position), gras: false })
  }

  return segments
}
