import { defineConfig } from 'vitepress'

export default defineConfig({
  title: "Task Management Docs",
  description: "Documentation for Task Management application",
  base: '/interview-c--todo-app/',
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' }
    ],

    sidebar: [
      {
        text: 'Architecture',
        items: [
          { text: 'ADRs', link: '/architecture/adrs/' }
        ]
      }
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/straiforos/interview-c--todo-app' }
    ]
  }
})
