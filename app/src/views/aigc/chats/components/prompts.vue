<template>
    <div class="ant-prompts css-var-r4c">
        <div class="ant-prompts-list ant-prompts-list-wrap">
            <div class="ant-prompts-item" v-for="(item, index) in presetQuestions" :key="index">
                <!-- <div class="ant-prompts-icon"><span role="img" aria-label="bulb" class="anticon anticon-bulb" style="color: rgb(255, 215, 0);"><svg viewBox="64 64 896 896" focusable="false" data-icon="bulb" width="1em" height="1em" fill="currentColor" aria-hidden="true">
                            <path d="M632 888H392c-4.4 0-8 3.6-8 8v32c0 17.7 14.3 32 32 32h192c17.7 0 32-14.3 32-32v-32c0-4.4-3.6-8-8-8zM512 64c-181.1 0-328 146.9-328 328 0 121.4 66 227.4 164 284.1V792c0 17.7 14.3 32 32 32h264c17.7 0 32-14.3 32-32V676.1c98-56.7 164-162.7 164-284.1 0-181.1-146.9-328-328-328zm127.9 549.8L604 634.6V752H420V634.6l-35.9-20.8C305.4 568.3 256 484.5 256 392c0-141.4 114.6-256 256-256s256 114.6 256 256c0 92.5-49.4 176.3-128.1 221.8z"></path>
                        </svg></span>
                </div> -->
                <div class="ant-prompts-content" @click="handlePrompt(item.content)">
                    <p class="ant-prompts-desc">{{ item.content }}</p>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name: 'prompts',
    props: {
        presetQuestions: {
            type: Array,
            default: () => [],
        },
    },
    data() {
        return {};
    },
    methods: {
        handlePrompt(content) {
            this.$emit('prompt', content);
        },
    },
};
</script>
<style>
.ant-prompts,
.ant-prompts * {
    box-sizing: border-box;
}

.ant-prompts {
    max-width: 100%;
    margin-bottom: var(--ant-padding-sm);
}

.ant-prompts .ant-prompts-list-wrap {
    display: flex;
    flex-wrap: wrap;
}

.ant-prompts .ant-prompts-list {
    display: flex;
    gap: var(--ant-padding-sm);
    list-style: none;
    padding-inline-start: 0;
    margin-block: 0;
    align-items: stretch;
    overflow: hidden;
}

.ant-prompts .ant-prompts-item {
    flex: none;
    display: flex;
    gap: var(--ant-padding-xs);
    height: auto;
    padding-block: var(--ant-padding-sm);
    padding-inline: var(--ant-padding);
    align-items: flex-start;
    justify-content: flex-start;
    background: var(--ant-color-bg-container);
    border-radius: var(--ant-border-radius-lg);
    transition: border var(--ant-motion-duration-slow),
        background var(--ant-motion-duration-slow);
    border: var(--ant-line-width) var(--ant-line-type)
        var(--ant-color-border-secondary);
    cursor: pointer;
}

.ant-prompts .ant-prompts-item .ant-prompts-icon,
.ant-prompts .ant-prompts-item .ant-prompts-label,
.ant-prompts .ant-prompts-item .ant-prompts-desc {
    margin: 0;
    padding: 0;
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    text-align: start;
    white-space: normal;
}
</style>
