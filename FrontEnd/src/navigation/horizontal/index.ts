import type { HorizontalNavItems } from '@layouts/types';

export default [
    {
        title: 'Home',
        to: { name: 'index' },
        icon: { icon: 'mdi-home-outline' },
    },
    {
        title: 'Second page',
        to: { name: 'second-page' },
        icon: { icon: 'mdi-file-document-outline' },
    },
    {
        title: 'Building page',
        to: { name: 'building-list' },
        icon: { icon: 'mdi-file-document-outline' },
    },
    {
        title: 'Playlist Generator',
        to: { name: 'playlist-generator' },
        icon: { icon: 'mdi-file-document-outline' },
    },
] as HorizontalNavItems;
