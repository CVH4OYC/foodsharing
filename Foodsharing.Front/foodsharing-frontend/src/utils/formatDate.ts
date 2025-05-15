export function formatDateHumanFriendly(iso: string): string {
    const date = new Date(iso);
    const now = new Date();
  
    const isSameDay = date.toDateString() === now.toDateString();
    const diffInMs = now.getTime() - date.getTime();
    const diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
  
    if (isSameDay) {
      return date.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' });
    }
  
    if (diffInDays === 1) return 'вчера';
    if (diffInDays === 2) return 'позавчера';
    if (diffInDays <= 3) return `${diffInDays} дня назад`;
  
    return date.toLocaleDateString('ru-RU');
  }
  