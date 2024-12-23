<template>
    <div class="ant-welcome css-var-r4c ant-welcome-filled ant-flex css-var-r4c">
        <div class="ant-welcome-icon">
            <el-icon size="64px"><sc-icon-bot /></el-icon>
        </div>
        <div class="ant-welcome-content-wrapper ant-flex css-var-r4c ant-flex-align-stretch ant-flex-vertical" v-html="renderMarkdown">

        </div>
    </div>
</template>
<script>
import markdownit from 'markdown-it';
export default {
    name: 'welcome',
    props: {
        introductionText: {
            type: String,
            default: '',
        },
    },
    data() {
        return {
            md: markdownit({
                breaks: true,
                html: true,
                linkify: true,
                typographer: true,
            }),
        };
    },
    computed: {
        renderMarkdown() {
            return this.md.render(this.introductionText);
        },
    },
};
</script>
<style>
.ant-welcome {
    gap: var(--ant-padding);
    width: 100%;
    margin-bottom: var(--ant-padding-sm);
}
.ant-welcome-filled {
    padding-inline: var(--ant-padding);
    padding-block: var(--ant-padding-sm);
    background: var(--ant-color-fill-content);
    border-radius: var(--ant-border-radius-lg);
}
.ant-welcome .ant-welcome-icon {
    flex-shrink: 0;
    height: calc(
        calc(var(--ant-font-size-heading-3) * var(--ant-line-height-heading-3)) +
            calc(var(--ant-font-size) * var(--ant-line-height)) +
            var(--ant-padding-xxs)
    );
    display: flex;
}
.ant-welcome .ant-welcome-icon img {
    height: 100%;
}
.ant-welcome .ant-welcome-content-wrapper {
    gap: var(--ant-padding-xs);
    flex: auto;
    min-width: 0;
}
.ant-flex-vertical {
    flex-direction: column;
}
.ant-flex-align-stretch {
    align-items: stretch;
}
</style>
