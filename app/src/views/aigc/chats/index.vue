<template>
    <el-main>
        <el-scrollbar ref="scrollbarRef">
            <welcome :introductionText="introductionText" />
            <prompts :presetQuestions="presetQuestions" @prompt="handleSendMessage"/>
            <bubble :item="item" v-for="item in localMessages" v-bind:key="item.id" />
        </el-scrollbar>

    </el-main>
    <el-footer style="height: auto; border-top: none;">
        <sender :loading="assistantLoading" :text="userPrompt" @send="handleSendMessage" @delete="handleClearData" />
    </el-footer>

</template>

<script>
import { reactive, nextTick } from 'vue';
import { fetchEventSource } from '@microsoft/fetch-event-source';
import config from '@/config';
import tool from '@/utils/tool';
import welcome from './components/welcome';
import prompts from './components/prompts';
import bubble from './components/bubble';
import conversations from './components/conversations';
import sender from './components/sender';
import { v4 as uuidv4 } from 'uuid';
export default {
    name: 'application',
    components: {
        welcome,
        prompts,
        bubble,
        conversations,
        sender,
    },
    props: {
        introductionText: {
            type: String,
            default: '',
        },
        presetQuestions: {
            type: Array,
            default: () => [],
        },
        messages: {
            type: Array,
            Required: true,
        },
        agentId: {
            type: String,
            default: '',
        },
    },
    data() {
        return {
            localMessages: reactive([...this.messages]),
            userPrompt: '',
            assistantLoading: false,
        };
    },
    methods: {
        async handleSendMessage(message) {
            this.userPrompt = message;
            let url = `${process.env.VUE_APP_API_BASEURL}${config.API_URL}/agent/AgentStart`;
            let token = tool.cookie.get('TOKEN');

            let headers = {
                'Content-Type': 'application/json',
                'Cache-Control': 'no-cache',
                Connection: 'keep-alive',
            };
            headers[config.TOKEN_NAME] = config.TOKEN_PREFIX + token;

            this.assistantLoading = true;
            let me = this;

            await fetchEventSource(url, {
                method: 'POST',
                openWhenHidden: true,
                headers: headers,
                body: JSON.stringify({
                    agentId: this.agentId,
                    userInput: this.userPrompt,
                    streaming: true,
                }),
                onopen() {
                    let id = uuidv4();
                    me.addUserMessage(id, me.userPrompt);
                    me.assistantLoading = true;
                },
                onmessage(ev) {
                    console.log('onmessage' + ev);
                    if (ev.event == 'message_template') {
                        let data = JSON.parse(ev.data);
                        me.addAssistantMessage(data.StepId, {
                            template: data.Content,
                        });
                    } else if (ev.event == 'data') {
                        let data = JSON.parse(ev.data);
                        me.addMessageContent(data.StepId, data.Content);
                        me.scrollToBottom();
                    }
                },
                onclose() {
                    console.log('onclose');
                    me.assistantLoading = false;
                    me.userPrompt = '';
                },
                onerror(err) {
                    me.assistantLoading = false;
                    throw err;
                },
            });
        },
        handleClearData() {
            this.localMessages = [];
        },
        addUserMessage(id, message) {
            this.localMessages.push({
                id: id,
                role: 'user',
                content: message,
                options: {},
                bubbleOptions: { end: true },
            });
        },
        addAssistantMessage(id, options) {
            this.localMessages.push({
                id: id,
                role: 'assistant',
                content: '',
                options: options,
                bubbleOptions: { start: true, footer: { copy: true } },
            });
        },
        addMessageContent(id, content) {
            let targetIndex = this.localMessages.findIndex((item) => item.id == id);
            if (targetIndex !== -1) {
                let item = this.localMessages[targetIndex];
                item.content += content;
            }
        },
        scrollToBottom() {
            nextTick(() => {
                const scrollbarRef = this.$refs.scrollbarRef;
                if (scrollbarRef) {
                    scrollbarRef.setScrollTop(scrollbarRef.$el.scrollHeight);
                    console.log(scrollbarRef.$el.scrollHeight);
                }
            });
        },
    },
};
</script>

<style scoped>
.css-var-r4c {
    --ant-blue: #1677ff;
    --ant-purple: #722ed1;
    --ant-cyan: #13c2c2;
    --ant-green: #52c41a;
    --ant-magenta: #eb2f96;
    --ant-pink: #eb2f96;
    --ant-red: #f5222d;
    --ant-orange: #fa8c16;
    --ant-yellow: #fadb14;
    --ant-volcano: #fa541c;
    --ant-geekblue: #2f54eb;
    --ant-gold: #faad14;
    --ant-lime: #a0d911;
    --ant-color-primary: #1677ff;
    --ant-color-success: #52c41a;
    --ant-color-warning: #faad14;
    --ant-color-error: #ff4d4f;
    --ant-color-info: #1677ff;
    --ant-color-link: #1677ff;
    --ant-color-text-base: #000;
    --ant-color-bg-base: #fff;
    --ant-font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto,
        'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji',
        'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji';
    --ant-font-family-code: 'SFMono-Regular', Consolas, 'Liberation Mono', Menlo,
        Courier, monospace;
    --ant-font-size: 14px;
    --ant-line-width: 1px;
    --ant-line-type: solid;
    --ant-motion-ease-out-circ: cubic-bezier(0.08, 0.82, 0.17, 1);
    --ant-motion-ease-in-out-circ: cubic-bezier(0.78, 0.14, 0.15, 0.86);
    --ant-motion-ease-out: cubic-bezier(0.215, 0.61, 0.355, 1);
    --ant-motion-ease-in-out: cubic-bezier(0.645, 0.045, 0.355, 1);
    --ant-motion-ease-out-back: cubic-bezier(0.12, 0.4, 0.29, 1.46);
    --ant-motion-ease-in-back: cubic-bezier(0.71, -0.46, 0.88, 0.6);
    --ant-motion-ease-in-quint: cubic-bezier(0.755, 0.05, 0.855, 0.06);
    --ant-motion-ease-out-quint: cubic-bezier(0.23, 1, 0.32, 1);
    --ant-border-radius: 6px;
    --ant-size-popup-arrow: 16px;
    --ant-control-height: 32px;
    --ant-z-index-base: 0;
    --ant-z-index-popup-base: 1000;
    --ant-opacity-image: 1;
    --ant-blue-1: #e6f4ff;
    --ant-blue-2: #bae0ff;
    --ant-blue-3: #91caff;
    --ant-blue-4: #69b1ff;
    --ant-blue-5: #4096ff;
    --ant-blue-6: #1677ff;
    --ant-blue-7: #0958d9;
    --ant-blue-8: #003eb3;
    --ant-blue-9: #002c8c;
    --ant-blue-10: #001d66;
    --ant-purple-1: #f9f0ff;
    --ant-purple-2: #efdbff;
    --ant-purple-3: #d3adf7;
    --ant-purple-4: #b37feb;
    --ant-purple-5: #9254de;
    --ant-purple-6: #722ed1;
    --ant-purple-7: #531dab;
    --ant-purple-8: #391085;
    --ant-purple-9: #22075e;
    --ant-purple-10: #120338;
    --ant-cyan-1: #e6fffb;
    --ant-cyan-2: #b5f5ec;
    --ant-cyan-3: #87e8de;
    --ant-cyan-4: #5cdbd3;
    --ant-cyan-5: #36cfc9;
    --ant-cyan-6: #13c2c2;
    --ant-cyan-7: #08979c;
    --ant-cyan-8: #006d75;
    --ant-cyan-9: #00474f;
    --ant-cyan-10: #002329;
    --ant-green-1: #f6ffed;
    --ant-green-2: #d9f7be;
    --ant-green-3: #b7eb8f;
    --ant-green-4: #95de64;
    --ant-green-5: #73d13d;
    --ant-green-6: #52c41a;
    --ant-green-7: #389e0d;
    --ant-green-8: #237804;
    --ant-green-9: #135200;
    --ant-green-10: #092b00;
    --ant-magenta-1: #fff0f6;
    --ant-magenta-2: #ffd6e7;
    --ant-magenta-3: #ffadd2;
    --ant-magenta-4: #ff85c0;
    --ant-magenta-5: #f759ab;
    --ant-magenta-6: #eb2f96;
    --ant-magenta-7: #c41d7f;
    --ant-magenta-8: #9e1068;
    --ant-magenta-9: #780650;
    --ant-magenta-10: #520339;
    --ant-pink-1: #fff0f6;
    --ant-pink-2: #ffd6e7;
    --ant-pink-3: #ffadd2;
    --ant-pink-4: #ff85c0;
    --ant-pink-5: #f759ab;
    --ant-pink-6: #eb2f96;
    --ant-pink-7: #c41d7f;
    --ant-pink-8: #9e1068;
    --ant-pink-9: #780650;
    --ant-pink-10: #520339;
    --ant-red-1: #fff1f0;
    --ant-red-2: #ffccc7;
    --ant-red-3: #ffa39e;
    --ant-red-4: #ff7875;
    --ant-red-5: #ff4d4f;
    --ant-red-6: #f5222d;
    --ant-red-7: #cf1322;
    --ant-red-8: #a8071a;
    --ant-red-9: #820014;
    --ant-red-10: #5c0011;
    --ant-orange-1: #fff7e6;
    --ant-orange-2: #ffe7ba;
    --ant-orange-3: #ffd591;
    --ant-orange-4: #ffc069;
    --ant-orange-5: #ffa940;
    --ant-orange-6: #fa8c16;
    --ant-orange-7: #d46b08;
    --ant-orange-8: #ad4e00;
    --ant-orange-9: #873800;
    --ant-orange-10: #612500;
    --ant-yellow-1: #feffe6;
    --ant-yellow-2: #ffffb8;
    --ant-yellow-3: #fffb8f;
    --ant-yellow-4: #fff566;
    --ant-yellow-5: #ffec3d;
    --ant-yellow-6: #fadb14;
    --ant-yellow-7: #d4b106;
    --ant-yellow-8: #ad8b00;
    --ant-yellow-9: #876800;
    --ant-yellow-10: #614700;
    --ant-volcano-1: #fff2e8;
    --ant-volcano-2: #ffd8bf;
    --ant-volcano-3: #ffbb96;
    --ant-volcano-4: #ff9c6e;
    --ant-volcano-5: #ff7a45;
    --ant-volcano-6: #fa541c;
    --ant-volcano-7: #d4380d;
    --ant-volcano-8: #ad2102;
    --ant-volcano-9: #871400;
    --ant-volcano-10: #610b00;
    --ant-geekblue-1: #f0f5ff;
    --ant-geekblue-2: #d6e4ff;
    --ant-geekblue-3: #adc6ff;
    --ant-geekblue-4: #85a5ff;
    --ant-geekblue-5: #597ef7;
    --ant-geekblue-6: #2f54eb;
    --ant-geekblue-7: #1d39c4;
    --ant-geekblue-8: #10239e;
    --ant-geekblue-9: #061178;
    --ant-geekblue-10: #030852;
    --ant-gold-1: #fffbe6;
    --ant-gold-2: #fff1b8;
    --ant-gold-3: #ffe58f;
    --ant-gold-4: #ffd666;
    --ant-gold-5: #ffc53d;
    --ant-gold-6: #faad14;
    --ant-gold-7: #d48806;
    --ant-gold-8: #ad6800;
    --ant-gold-9: #874d00;
    --ant-gold-10: #613400;
    --ant-lime-1: #fcffe6;
    --ant-lime-2: #f4ffb8;
    --ant-lime-3: #eaff8f;
    --ant-lime-4: #d3f261;
    --ant-lime-5: #bae637;
    --ant-lime-6: #a0d911;
    --ant-lime-7: #7cb305;
    --ant-lime-8: #5b8c00;
    --ant-lime-9: #3f6600;
    --ant-lime-10: #254000;
    --ant-color-text: rgba(0, 0, 0, 0.88);
    --ant-color-text-secondary: rgba(0, 0, 0, 0.65);
    --ant-color-text-tertiary: rgba(0, 0, 0, 0.45);
    --ant-color-text-quaternary: rgba(0, 0, 0, 0.25);
    --ant-color-fill: rgba(0, 0, 0, 0.15);
    --ant-color-fill-secondary: rgba(0, 0, 0, 0.06);
    --ant-color-fill-tertiary: rgba(0, 0, 0, 0.04);
    --ant-color-fill-quaternary: rgba(0, 0, 0, 0.02);
    --ant-color-bg-solid: rgb(0, 0, 0);
    --ant-color-bg-solid-hover: rgba(0, 0, 0, 0.75);
    --ant-color-bg-solid-active: rgba(0, 0, 0, 0.95);
    --ant-color-bg-layout: #f5f5f5;
    --ant-color-bg-container: #ffffff;
    --ant-color-bg-elevated: #ffffff;
    --ant-color-bg-spotlight: rgba(0, 0, 0, 0.85);
    --ant-color-bg-blur: transparent;
    --ant-color-border: #d9d9d9;
    --ant-color-border-secondary: #f0f0f0;
    --ant-color-primary-bg: #e6f4ff;
    --ant-color-primary-bg-hover: #bae0ff;
    --ant-color-primary-border: #91caff;
    --ant-color-primary-border-hover: #69b1ff;
    --ant-color-primary-hover: #4096ff;
    --ant-color-primary-active: #0958d9;
    --ant-color-primary-text-hover: #4096ff;
    --ant-color-primary-text: #1677ff;
    --ant-color-primary-text-active: #0958d9;
    --ant-color-success-bg: #f6ffed;
    --ant-color-success-bg-hover: #d9f7be;
    --ant-color-success-border: #b7eb8f;
    --ant-color-success-border-hover: #95de64;
    --ant-color-success-hover: #95de64;
    --ant-color-success-active: #389e0d;
    --ant-color-success-text-hover: #73d13d;
    --ant-color-success-text: #52c41a;
    --ant-color-success-text-active: #389e0d;
    --ant-color-error-bg: #fff2f0;
    --ant-color-error-bg-hover: #fff1f0;
    --ant-color-error-bg-filled-hover: #ffdfdc;
    --ant-color-error-bg-active: #ffccc7;
    --ant-color-error-border: #ffccc7;
    --ant-color-error-border-hover: #ffa39e;
    --ant-color-error-hover: #ff7875;
    --ant-color-error-active: #d9363e;
    --ant-color-error-text-hover: #ff7875;
    --ant-color-error-text: #ff4d4f;
    --ant-color-error-text-active: #d9363e;
    --ant-color-warning-bg: #fffbe6;
    --ant-color-warning-bg-hover: #fff1b8;
    --ant-color-warning-border: #ffe58f;
    --ant-color-warning-border-hover: #ffd666;
    --ant-color-warning-hover: #ffd666;
    --ant-color-warning-active: #d48806;
    --ant-color-warning-text-hover: #ffc53d;
    --ant-color-warning-text: #faad14;
    --ant-color-warning-text-active: #d48806;
    --ant-color-info-bg: #e6f4ff;
    --ant-color-info-bg-hover: #bae0ff;
    --ant-color-info-border: #91caff;
    --ant-color-info-border-hover: #69b1ff;
    --ant-color-info-hover: #69b1ff;
    --ant-color-info-active: #0958d9;
    --ant-color-info-text-hover: #4096ff;
    --ant-color-info-text: #1677ff;
    --ant-color-info-text-active: #0958d9;
    --ant-color-link-hover: #69b1ff;
    --ant-color-link-active: #0958d9;
    --ant-color-bg-mask: rgba(0, 0, 0, 0.45);
    --ant-color-white: #fff;
    --ant-font-size-sm: 12px;
    --ant-font-size-lg: 16px;
    --ant-font-size-xl: 20px;
    --ant-font-size-heading-1: 38px;
    --ant-font-size-heading-2: 30px;
    --ant-font-size-heading-3: 24px;
    --ant-font-size-heading-4: 20px;
    --ant-font-size-heading-5: 16px;
    --ant-line-height: 1.5714285714285714;
    --ant-line-height-lg: 1.5;
    --ant-line-height-sm: 1.6666666666666667;
    --ant-font-height: 22px;
    --ant-font-height-lg: 24px;
    --ant-font-height-sm: 20px;
    --ant-line-height-heading-1: 1.2105263157894737;
    --ant-line-height-heading-2: 1.2666666666666666;
    --ant-line-height-heading-3: 1.3333333333333333;
    --ant-line-height-heading-4: 1.4;
    --ant-line-height-heading-5: 1.5;
    --ant-control-height-sm: 24px;
    --ant-control-height-xs: 16px;
    --ant-control-height-lg: 40px;
    --ant-motion-duration-fast: 0.1s;
    --ant-motion-duration-mid: 0.2s;
    --ant-motion-duration-slow: 0.3s;
    --ant-line-width-bold: 2px;
    --ant-border-radius-xs: 2px;
    --ant-border-radius-sm: 4px;
    --ant-border-radius-lg: 8px;
    --ant-border-radius-outer: 4px;
    --ant-color-fill-content: rgba(0, 0, 0, 0.06);
    --ant-color-fill-content-hover: rgba(0, 0, 0, 0.15);
    --ant-color-fill-content-end: rgba(148, 152, 247, 0.44);
    --ant-color-fill-alter: rgba(0, 0, 0, 0.02);
    --ant-color-bg-container-disabled: rgba(0, 0, 0, 0.04);
    --ant-color-border-bg: #ffffff;
    --ant-color-split: rgba(5, 5, 5, 0.06);
    --ant-color-text-placeholder: rgba(0, 0, 0, 0.25);
    --ant-color-text-disabled: rgba(0, 0, 0, 0.25);
    --ant-color-text-heading: rgba(0, 0, 0, 0.88);
    --ant-color-text-label: rgba(0, 0, 0, 0.65);
    --ant-color-text-description: rgba(0, 0, 0, 0.45);
    --ant-color-text-light-solid: #fff;
    --ant-color-highlight: #ff4d4f;
    --ant-color-bg-text-hover: rgba(0, 0, 0, 0.06);
    --ant-color-bg-text-active: rgba(0, 0, 0, 0.15);
    --ant-color-icon: rgba(0, 0, 0, 0.45);
    --ant-color-icon-hover: rgba(0, 0, 0, 0.88);
    --ant-color-error-outline: rgba(255, 38, 5, 0.06);
    --ant-color-warning-outline: rgba(255, 215, 5, 0.1);
    --ant-font-size-icon: 12px;
    --ant-line-width-focus: 3px;
    --ant-control-outline-width: 2px;
    --ant-control-interactive-size: 16px;
    --ant-control-item-bg-hover: rgba(0, 0, 0, 0.04);
    --ant-control-item-bg-active: #e6f4ff;
    --ant-control-item-bg-active-hover: #bae0ff;
    --ant-control-item-bg-active-disabled: rgba(0, 0, 0, 0.15);
    --ant-control-tmp-outline: rgba(0, 0, 0, 0.02);
    --ant-control-outline: rgba(5, 145, 255, 0.1);
    --ant-font-weight-strong: 600;
    --ant-opacity-loading: 0.65;
    --ant-link-decoration: none;
    --ant-link-hover-decoration: none;
    --ant-link-focus-decoration: none;
    --ant-control-padding-horizontal: 12px;
    --ant-control-padding-horizontal-sm: 8px;
    --ant-padding-xxs: 4px;
    --ant-padding-xs: 8px;
    --ant-padding-sm: 12px;
    --ant-padding: 16px;
    --ant-padding-md: 20px;
    --ant-padding-lg: 24px;
    --ant-padding-xl: 32px;
    --ant-padding-content-horizontal-lg: 24px;
    --ant-padding-content-vertical-lg: 16px;
    --ant-padding-content-horizontal: 16px;
    --ant-padding-content-vertical: 12px;
    --ant-padding-content-horizontal-sm: 16px;
    --ant-padding-content-vertical-sm: 8px;
    --ant-margin-xxs: 4px;
    --ant-margin-xs: 8px;
    --ant-margin-sm: 12px;
    --ant-margin: 16px;
    --ant-margin-md: 20px;
    --ant-margin-lg: 24px;
    --ant-margin-xl: 32px;
    --ant-margin-xxl: 48px;
    --ant-box-shadow: 0 6px 16px 0 rgba(0, 0, 0, 0.08),
        0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 9px 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-secondary: 0 6px 16px 0 rgba(0, 0, 0, 0.08),
        0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 9px 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-tertiary: 0 1px 2px 0 rgba(0, 0, 0, 0.03),
        0 1px 6px -1px rgba(0, 0, 0, 0.02), 0 2px 4px 0 rgba(0, 0, 0, 0.02);
    --ant-box-shadow-popover-arrow: 2px 2px 5px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-card: 0 1px 2px -2px rgba(0, 0, 0, 0.16),
        0 3px 6px 0 rgba(0, 0, 0, 0.12), 0 5px 12px 4px rgba(0, 0, 0, 0.09);
    --ant-box-shadow-drawer-right: -6px 0 16px 0 rgba(0, 0, 0, 0.08),
        -3px 0 6px -4px rgba(0, 0, 0, 0.12), -9px 0 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-drawer-left: 6px 0 16px 0 rgba(0, 0, 0, 0.08),
        3px 0 6px -4px rgba(0, 0, 0, 0.12), 9px 0 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-drawer-up: 0 6px 16px 0 rgba(0, 0, 0, 0.08),
        0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 9px 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-drawer-down: 0 -6px 16px 0 rgba(0, 0, 0, 0.08),
        0 -3px 6px -4px rgba(0, 0, 0, 0.12), 0 -9px 28px 8px rgba(0, 0, 0, 0.05);
    --ant-box-shadow-tabs-overflow-left: inset 10px 0 8px -8px rgba(0, 0, 0, 0.08);
    --ant-box-shadow-tabs-overflow-right: inset -10px 0 8px -8px
        rgba(0, 0, 0, 0.08);
    --ant-box-shadow-tabs-overflow-top: inset 0 10px 8px -8px rgba(0, 0, 0, 0.08);
    --ant-box-shadow-tabs-overflow-bottom: inset 0 -10px 8px -8px
        rgba(0, 0, 0, 0.08);
}

.ant-space-horizontal {
    display: inline-flex;
    flex-direction: row;
}
.ant-space-align-center {
    align-items: center;
}
.ant-btn-icon-xxs {
    padding: var(--ant-padding-xxs);
    font-size: var(--ant-font-size);
}
.ant-btn-icon-sm {
    padding: var(--ant-padding-sm);
    font-size: var(--ant-size-popup-arrow);
}
.ant-typography {
    --ant-typography-title-margin-top: 1.2em;
    --ant-typography-title-margin-bottom: 0.5em;
}
.ant-typography.ant-typography-secondary {
    color: var(--ant-color-text-description);
}
.ant-badge {
    --ant-badge-indicator-z-index: auto;
    --ant-badge-indicator-height: 20px;
    --ant-badge-indicator-height-sm: 14px;
    --ant-badge-dot-size: 6px;
    --ant-badge-text-font-size: 12px;
    --ant-badge-text-font-size-sm: 12px;
    --ant-badge-text-font-weight: normal;
    --ant-badge-status-size: 6px;
}
.ant-input-css-var {
    --ant-input-padding-block: 4px;
    --ant-input-padding-block-sm: 0px;
    --ant-input-padding-block-lg: 7px;
    --ant-input-padding-inline: 11px;
    --ant-input-padding-inline-sm: 7px;
    --ant-input-padding-inline-lg: 11px;
    --ant-input-addon-bg: rgba(0, 0, 0, 0.02);
    --ant-input-active-border-color: #1677ff;
    --ant-input-hover-border-color: #4096ff;
    --ant-input-active-shadow: 0 0 0 2px rgba(5, 145, 255, 0.1);
    --ant-input-error-active-shadow: 0 0 0 2px rgba(255, 38, 5, 0.06);
    --ant-input-warning-active-shadow: 0 0 0 2px rgba(255, 215, 5, 0.1);
    --ant-input-hover-bg: #ffffff;
    --ant-input-active-bg: #ffffff;
    --ant-input-input-font-size: 14px;
    --ant-input-input-font-size-lg: 16px;
    --ant-input-input-font-size-sm: 14px;
}
.el-button + .el-button {
    margin-left: 5px;
}
.ant-typography {
    --ant-typography-title-margin-top: 1.2em;
    --ant-typography-title-margin-bottom: 0.5em;
}
.ant-space-gap-col-small {
    column-gap: var(--ant-padding-xs);
}
.ant-space-gap-row-small {
    row-gap: var(--ant-padding-xs);
}
.ant-space-align-center {
    align-items: center;
}
.ant-space {
    display: inline-flex;
}
.anticon {
    display: inline-flex;
    align-items: center;
    color: inherit;
    font-style: normal;
    line-height: 0;
    text-align: center;
    text-transform: none;
    vertical-align: -0.125em;
    text-rendering: optimizeLegibility;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
}
.ant-flex {
    display: flex;
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
::v-deep ol,
::v-deep ul {
    margin: 1em 0;
    padding-left: 2em;
}

::v-deep li {
    font-size: 1em;
    border-radius: 0.3em;
    padding: 0.2em 0.4em;
    color: #24292e;
    margin-bottom: 0.5em;
}

::v-deep ol {
    list-style-type: decimal;
}

::v-deep ul {
    list-style-type: disc;
}

::v-deep p {
    margin: 0.5em 0;
    line-height: 1.6;
}

::v-deep h1,
::v-deep h2,
::v-deep h3,
::v-deep h4,
::v-deep h5,
::v-deep h6 {
    font-weight: bold;
    color: #24292e;
}

::v-deep h1 {
    font-size: 2em;
    border-bottom: 2px solid #e1e4e8;
    padding-bottom: 0.3em;
}

::v-deep h2 {
    font-size: 1.75em;
    border-bottom: 1px solid #e1e4e8;
    padding-bottom: 0.3em;
}

::v-deep h3 {
    font-size: 1.5em !important;
}

::v-deep h4 {
    font-size: 1.25em;
}

::v-deep h5 {
    font-size: 1em;
}

::v-deep h6 {
    font-size: 0.875em;
}

::v-deep blockquote {
    margin: 1em 0;
    padding: 0.5em 1em;
    background-color: #f6f8fa;
    border-left: 5px solid #e1e4e8;
    color: #6a737d;
}

::v-deep pre {
    border-radius: 10px;
}
::v-deep code {
    font-family: ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas,
        Liberation Mono, monospace !important;
    padding: 0.2em 0.4em;
    border-radius: 0.3em;
}

::v-deep code:not(pre code) {
    padding: 0px;
    font-weight: 600;
}

::v-deep pre code {
    display: block;
    overflow: auto;
    font-size: 1em;
    padding: 1em;
    border-radius: 1px;
}

::v-deep a {
    color: #0366d6;
    text-decoration: none;
}
::v-deep a:hover {
    text-decoration: underline;
}
</style>
