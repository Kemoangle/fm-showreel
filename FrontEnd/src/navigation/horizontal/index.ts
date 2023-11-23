import type { HorizontalNavItems } from '@layouts/types'

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
    to: { name: 'videos-list' },
    icon: { icon: 'mdi-videocam-outline' },
  }
] as HorizontalNavItems
