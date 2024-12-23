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
<style>

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
}
.ant-bubble .ant-bubble-content-filled-end {
    background-color: var(--ant-color-fill-content-end);
    padding: var(--ant-padding-sm) var(--ant-padding);
    border-radius: var(--ant-border-radius-lg);
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
</style>