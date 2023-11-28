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
        icon: { icon: 'mdi-office-building-settings' },
    },
    {
        title: 'Video page',
        icon: { icon: 'mdi-videocam-outline' },
        children: [
          { title: 'Video', to: 'videos-list' },
          { title: 'Category', to: { name: 'videos-category-category'} },
        ],
    },
    {
        title: 'Playlist Generator',
        to: { name: 'playlist-generator' },
        icon: { icon: 'mdi-file-document-outline' },
    },
] as HorizontalNavItems;
