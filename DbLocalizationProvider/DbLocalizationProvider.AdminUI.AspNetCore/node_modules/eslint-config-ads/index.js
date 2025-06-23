module.exports = {
    extends: [
        'eslint:recommended',
    ],

    rules: {
        'array-bracket-newline': [
            'error',
            {
                minItems: 1,
            },
        ],
        'array-element-newline': [
            'error',
            'always',
        ],
        'comma-dangle': [
            'error',
            'always-multiline',
        ],
        indent: [
            'error',
            4,
        ],
        'key-spacing': 'error',
        'object-property-newline': 'error',
        'object-curly-newline': [
            'error',
            {
                ObjectExpression: {
                    minProperties: 1,
                },
                ObjectPattern: {
                    multiline: true,
                },
                ImportDeclaration: {
                    multiline: true,
                },
                ExportDeclaration: {
                    multiline: true,
                },
            },
        ],
        'no-extra-semi': 'error',
        semi: [
            'error',
            'always',
        ],
        'semi-spacing': [
            'error',
            {
                before: false,
                after: true,
            },
        ],
        'quote-props': [
            'error',
            'as-needed',
        ],
        quotes: [
            'error',
            'single',
        ],
        'no-unused-expressions': 'error',
        'no-alert': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'function-paren-newline': [
            'error',
            'multiline',
        ],
        'space-before-function-paren': [
            'error',
            'always',
        ],
    },

    parserOptions: {
        ecmaVersion: 6,
    },
};
