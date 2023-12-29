import type { HorizontalNavItems } from '@layouts/types';

export default [
    {
        title: 'Home',
        to: { name: 'index' },
        icon: { icon: 'mdi-home-outline' },
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
            { title: 'Video Category', to: { name: 'videos-category' } },
            { title: 'Video List', to: { name: 'videos-videoList-videoList' } },
        ],
    },
    {
        title: 'Playlist page',
        icon: { icon: 'mdi-file-document-outline' },
        children: [
            {
                title: 'Playlist',
                to: { name: 'playlist-list' },
            },
            {
                title: 'Generator',
                to: { name: 'playlist-generator' },
            },
        ],
    },
] as HorizontalNavItems;
