module.exports = {
    root: true,
    env: {
        node: true,
    },
    extends: [
        'plugin:vue/vue3-essential',
        'eslint:recommended',
        '@vue/typescript/recommended',
        'plugin:prettier/recommended',
    ],
    parserOptions: {
        ecmaVersion: 2020,
    },
    rules: {
        'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'prettier/prettier': [
            'error',
            {
                endOfLine: 'auto',
                arrowParens: 'always',
                bracketSameLine: false,
                bracketSpacing: true,
                embeddedLanguageFormatting: 'auto',
                htmlWhitespaceSensitivity: 'css',
                insertPragma: false,
                jsxSingleQuote: false,
                printWidth: 100,
                proseWrap: 'preserve',
                quoteProps: 'as-needed',
                requirePragma: false,
                semi: true,
                singleQuote: false,
                tabWidth: 4,
                trailingComma: 'es5',
                useTabs: false,
                vueIndentScriptAndStyle: false,
            },
        ],
    },
};
