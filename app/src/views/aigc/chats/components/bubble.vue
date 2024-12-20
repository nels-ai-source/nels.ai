<template>
    <div :class="['ant-bubble', 'css-var-r4c', bubbleClass]">
        <div class="ant-bubble-avatar">
            <el-avatar :size="this.avatarSize" :icon="this.avatarIcon"></el-avatar>
        </div>
        <div :class="bubbleWrapperClass">
            <div class="ant-bubble-header" v-if="this.processedData.bubbleOptions.header">{{this.processedData.bubbleOptions.header}}</div>
            <div :class="['ant-bubble-content',bubbleContentClass]" v-html="renderMarkdown">

            </div>
            <div class="ant-bubble-footer" v-if="processedData.bubbleOptions.footer">
                <div class="ant-space ant-space-horizontal ant-space-align-center css-var-r4c" style="gap: 4px;">
                    <div class="ant-space-item" v-if="processedData.bubbleOptions.footer.copy">
                        <el-button text size="small" icon="el-icon-document-copy" class="ant-btn-icon-xxs" @click="handleCopy" />
                    </div>
                    <div class="ant-space-item" v-if="processedData.bubbleOptions.footer.refresh">
                        <el-button text size="small" icon="el-icon-refresh" class="ant-btn-icon-xxs" @click="handleRefresh" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import markdownit from 'markdown-it';
import hljs from 'highlight.js';
import 'highlight.js/styles/github-dark.css';
export default {
    name: 'bubble',
    components: {},
    props: {
        item: {
            type: Object,
            Required: true,
        },
        avatarSize: {
            type: Number,
            default: 32,
        },
        avatarIcon: {
            type: String,
            default: 'el-icon-user',
        },
    },
    data() {
        return {
            md: markdownit({
                breaks: true,
                html: true,
                linkify: true,
                typographer: true,
                highlight(str, lang) {
                    if (lang && hljs.getLanguage(lang)) {
                        try {
                            return hljs.highlight(str, { language: lang })
                                .value;
                        } catch (__) {
                            console.error('Error highlighting code:', __);
                        }
                    }
                    return '';
                },
            }),
        };
    },
    computed: {
        bubbleClass() {
            if (this.item?.bubbleOptions?.start) {
                return 'ant-bubble-start';
            }
            return 'ant-bubble-end';
        },
        bubbleWrapperClass() {
            if (this.item?.bubbleOptions?.start) {
                return 'ant-bubble-content-wrapper';
            }
            return '';
        },
        bubbleContentClass() {
            if (this.item?.bubbleOptions?.start) {
                return 'ant-bubble-content-filled';
            }
            return 'ant-bubble-content-filled-end';
        },
        bubbleContentShadowClass() {
            if (this.item?.bubbleOptions?.start) {
                return 'ant-bubble-content-shadow';
            }
            return 'ant-bubble-content-shadow-end';
        },
        processedData() {
            return this.item;
        },
        renderMarkdown() {
            return this.md.render(this.item.content);
        },
    },
    methods: {
        handleRefresh() {
            this.$emit('refresh');
        },
        handleCopy() {
            navigator.clipboard.writeText(this.item.content);
            this.$message.success('复制成功');
        },
    },
};
</script>
<style scoped>
.ant-bubble {
    display: flex;
    column-gap: var(--ant-padding-sm);
    max-width: 100%;
    margin-bottom: var(--ant-padding-sm);
}
.ant-bubble .ant-bubble-avatar {
    display: inline-flex;
    justify-content: center;
    align-self: flex-start;
}
.ant-bubble .ant-bubble-header {
    margin-bottom: var(--ant-padding-xxs);
}
.ant-bubble .ant-bubble-header,
.ant-bubble .ant-bubble-footer {
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    color: var(--ant-color-text);
    margin-top: var(--ant-margin-xxs);
}
.ant-bubble .ant-bubble-content-wrapper {
    flex: auto;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
}
.ant-bubble .ant-bubble-content-filled {
    background-color: var(--ant-color-fill-content);
    padding: var(--ant-padding-sm) var(--ant-padding);
    border-radius: var(--ant-border-radius-lg);
    border-bottom-left-radius: 0;
}
.ant-bubble .ant-bubble-content-filled-end {
    background-color: var(--ant-color-fill-content-end);
    padding: var(--ant-padding-sm) var(--ant-padding);
    border-radius: var(--ant-border-radius-lg);
    border-bottom-right-radius: 0;
}
.ant-bubble .ant-bubble-content {
    position: relative;
    box-sizing: border-box;
    color: var(--ant-color-text);
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    min-height: calc(
        var(--ant-padding-sm) * 2 + var(--ant-line-height) *
            var(--ant-font-size)
    );
    word-break: break-word;
}
.ant-bubble .ant-bubble-header,
.ant-bubble .ant-bubble-footer {
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    color: var(--ant-color-text);
}
.ant-bubble.ant-bubble-end {
    justify-content: end;
    flex-direction: row-reverse;
}
.ant-btn-icon-xxs {
    padding: var(--ant-padding-xxs);
    font-size: var(--ant-font-size);
}

.ant-bubble ::v-deep {
    font-family: ui-sans-serif, -apple-system, system-ui, Segoe UI, Roboto,
        Ubuntu, Cantarell, Noto Sans, sans-serif, Helvetica, Apple Color Emoji,
        Arial, Segoe UI Emoji, Segoe UI Symbol;
    font-size: 1em;
    color: #000000;
}
.ant-bubble ::v-deep ol,
.ant-bubble ::v-deep ul {
    margin: 1em 0;
    padding-left: 2em;
}

.ant-bubble ::v-deep li {
    font-size: 1em;
    border-radius: 0.3em;
    padding: 0.2em 0.4em;
    color: #24292e;
    margin-bottom: 0.5em;
}

.ant-bubble ::v-deep ol {
    list-style-type: decimal;
}

.ant-bubble ::v-deep ul {
    list-style-type: disc;
}

.ant-bubble ::v-deep p {
    margin: 0.5em 0;
    line-height: 1.6;
}

.ant-bubble ::v-deep h1,
.ant-bubble ::v-deep h2,
.ant-bubble ::v-deep h3,
.ant-bubble ::v-deep h4,
.ant-bubble ::v-deep h5,
.ant-bubble ::v-deep h6 {
    margin: 1em 0 0.5em;
    font-weight: bold;
    color: #24292e;
}

.ant-bubble ::v-deep h1 {
    font-size: 2em;
    border-bottom: 2px solid #e1e4e8;
    padding-bottom: 0.3em;
}

.ant-bubble ::v-deep h2 {
    font-size: 1.75em;
    border-bottom: 1px solid #e1e4e8;
    padding-bottom: 0.3em;
}

.ant-bubble ::v-deep h3 {
    font-size: 1.5em !important;
}

.ant-bubble ::v-deep h4 {
    font-size: 1.25em;
}

.ant-bubble ::v-deep h5 {
    font-size: 1em;
}

.ant-bubble ::v-deep h6 {
    font-size: 0.875em;
}

.ant-bubble ::v-deep blockquote {
    margin: 1em 0;
    padding: 0.5em 1em;
    background-color: #f6f8fa;
    border-left: 5px solid #e1e4e8;
    color: #6a737d;
}

.ant-bubble ::v-deep pre {
    border-radius: 10px;
}

.ant-bubble ::v-deep code {
    font-family: ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas,
        Liberation Mono, monospace !important;
    padding: 0.2em 0.4em;
    border-radius: 0.3em;
}

.ant-bubble ::v-deep code:not(pre code) {
    padding: 0px;
    font-weight: 600;
}

.ant-bubble ::v-deep pre code {
    display: block;
    overflow: auto;
    font-size: 1em;
    padding: 1em;
    border-radius: 1px;
}

.ant-bubble ::v-deep a {
    color: #0366d6;
    text-decoration: none;
}

.ant-bubble ::v-deep a:hover {
    text-decoration: underline;
}
</style>