<template>
    <el-main class="nopadding" style="height: 100%;">
        <el-container>
            <el-aside width="300px">

                <ul style="width:256px;background:#ffffff;border-radius:6px" class="ant-conversations css-var-r4c ">
                    <li>
                        <div class="ant-conversations-group-title"><span class="ant-typography css-var-r4c ">
                                <div class="ant-space ant-space-horizontal ant-space-align-center ant-space-gap-row-small ant-space-gap-col-small css-var-r4c ">
                                    <div class="ant-space-item"><span role="img" aria-label="comment" class="anticon anticon-comment">
                                            <svg viewBox="64 64 896 896" focusable="false" data-icon="comment" width="1em" height="1em" fill="currentColor" aria-hidden="true">

                                                <path d="M573 421c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40zm-280 0c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40z"></path>
                                                <path d="M894 345a343.92 343.92 0 00-189-130v.1c-17.1-19-36.4-36.5-58-52.1-163.7-119-393.5-82.7-513 81-96.3 133-92.2 311.9 6 439l.8 132.6c0 3.2.5 6.4 1.5 9.4a31.95 31.95 0 0040.1 20.9L309 806c33.5 11.9 68.1 18.7 102.5 20.6l-.5.4c89.1 64.9 205.9 84.4 313 49l127.1 41.4c3.2 1 6.5 1.6 9.9 1.6 17.7 0 32-14.3 32-32V753c88.1-119.6 90.4-284.9 1-408zM323 735l-12-5-99 31-1-104-8-9c-84.6-103.2-90.2-251.9-11-361 96.4-132.2 281.2-161.4 413-66 132.2 96.1 161.5 280.6 66 412-80.1 109.9-223.5 150.5-348 102zm505-17l-8 10 1 104-98-33-12 5c-56 20.8-115.7 22.5-171 7l-.2-.1A367.31 367.31 0 00729 676c76.4-105.3 88.8-237.6 44.4-350.4l.6.4c23 16.5 44.1 37.1 62 62 72.6 99.6 68.5 235.2-8 330z"></path>
                                                <path d="M433 421c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40z"></path>
                                            </svg>
                                        </span></div>
                                    <div class="ant-space-item"><span>Today</span></div>
                                </div>
                            </span></div>
                        <ul class="ant-conversations-list">
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204800000">Conversation 1732204800000</span></li>
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204803600">Conversation 1732204803600</span></li>
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204807200">Conversation 1732204807200</span></li>
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204810800">Conversation 1732204810800</span></li>
                        </ul>
                    </li>
                    <li>
                        <div class="ant-conversations-group-title"><span class="ant-typography css-var-r4c ">
                                <div class="ant-space ant-space-horizontal ant-space-align-center ant-space-gap-row-small ant-space-gap-col-small css-var-r4c ">
                                    <div class="ant-space-item"><span role="img" aria-label="comment" class="anticon anticon-comment"><svg viewBox="64 64 896 896" focusable="false" data-icon="comment" width="1em" height="1em" fill="currentColor" aria-hidden="true">

                                                <path d="M573 421c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40zm-280 0c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40z"></path>
                                                <path d="M894 345a343.92 343.92 0 00-189-130v.1c-17.1-19-36.4-36.5-58-52.1-163.7-119-393.5-82.7-513 81-96.3 133-92.2 311.9 6 439l.8 132.6c0 3.2.5 6.4 1.5 9.4a31.95 31.95 0 0040.1 20.9L309 806c33.5 11.9 68.1 18.7 102.5 20.6l-.5.4c89.1 64.9 205.9 84.4 313 49l127.1 41.4c3.2 1 6.5 1.6 9.9 1.6 17.7 0 32-14.3 32-32V753c88.1-119.6 90.4-284.9 1-408zM323 735l-12-5-99 31-1-104-8-9c-84.6-103.2-90.2-251.9-11-361 96.4-132.2 281.2-161.4 413-66 132.2 96.1 161.5 280.6 66 412-80.1 109.9-223.5 150.5-348 102zm505-17l-8 10 1 104-98-33-12 5c-56 20.8-115.7 22.5-171 7l-.2-.1A367.31 367.31 0 00729 676c76.4-105.3 88.8-237.6 44.4-350.4l.6.4c23 16.5 44.1 37.1 62 62 72.6 99.6 68.5 235.2-8 330z"></path>
                                                <path d="M433 421c-23.1 0-41 17.9-41 40s17.9 40 41 40c21.1 0 39-17.9 39-40s-17.9-40-39-40z"></path>
                                            </svg></span></div>
                                    <div class="ant-space-item"><span>Yesterday</span></div>
                                </div>
                            </span></div>
                        <ul class="ant-conversations-list">
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204728000">Conversation 1732204728000</span></li>
                            <li class="ant-conversations-item"><span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " aria-label="Conversation 1732204731600">Conversation 1732204731600</span></li>
                        </ul>
                    </li>
                </ul>

            </el-aside>
            <el-container>
                <el-main>
                    <div class="page-content" style=" padding: 10px">
                        <div class="ant-bubble css-var-r4c ant-bubble-start">
                            <div class="ant-bubble-avatar">
                                <el-avatar :size="32" icon="el-icon-user"></el-avatar>
                            </div>
                            <div class="ant-bubble-content-wrapper">
                                <div class="ant-bubble-header">Ant Design X</div>
                                <div class="ant-bubble-content ant-bubble-content-filled">Hello, welcome to use Ant Design X! Just ask if you have any questions.</div>
                                <div class="ant-bubble-footer">
                                    <div class="ant-space ant-space-horizontal ant-space-align-center css-var-r4c" style="gap: 4px;">
                                        <div class="ant-space-item">
                                            <el-button text size="small" icon="el-icon-refresh" class="ant-btn css-var-r4c ant-btn-icon-xxs" />
                                        </div>
                                        <div class="ant-space-item">
                                            <el-button text size="small" icon="el-icon-document-copy" class="ant-btn-icon-xxs" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ant-bubble css-var-r4c ant-bubble-end">
                            <div class="ant-bubble-avatar">
                                <el-avatar :size="32" icon="el-icon-user"></el-avatar>
                            </div>
                            <div class="ant-bubble-content ant-bubble-content-filled">Hi, good morning, I'm fine!</div>
                        </div>
                    </div>
                </el-main>
                <el-footer style="height: auto; border-top: none;">
                    <div class="ant-sender css-var-r4c">
                        <div class="ant-sender-header" style="">
                            <div class="ant-sender-header-header">
                                <div class="ant-sender-header-title">
                                    <div class="ant-space ant-space-horizontal ant-space-align-center ant-space-gap-row-small ant-space-gap-col-small css-var-rft">
                                        <div class="ant-space-item">
                                            <el-icon> <el-icon-switch /> </el-icon>
                                        </div>
                                        <div class="ant-space-item"><span class="ant-typography ant-typography-secondary css-var-rft">"Tell more about Ant Design X"</span></div>
                                    </div>
                                </div>
                                <div class="ant-sender-header-close">
                                    <el-button text icon="el-icon-close" class="ant-btn css-var-r4c ant-btn-icon-xxs" />
                                </div>
                            </div>
                        </div>
                        <div class="ant-sender-content">
                            <div class="ant-sender-prefix">
                                <span class="ant-badge css-var-r4c">
                                    <el-button icon="el-icon-link" class="ant-btn css-var-r4c ant-btn-icon-sm" />
                                    <el-button text icon="el-icon-delete" class="ant-btn css-var-r4c ant-btn-icon-sm" />
                                </span>
                            </div>
                            <el-input v-model="text" :rows="2" type="textarea" clearable placeholder="Please input" :autosize="{ minRows: 1, maxRows: 5 }" class="ant-input-textarea ant-input-textarea-show-count ant-input-textarea-with-normal ant-input-textarea-with-border ant-input-textarea-with-placeholder" />
                            <div class="ant-sender-actions-list">
                                <div class="ant-sender-actions-list-presets ant-flex css-var-r4c">
                                    <el-button text icon="el-icon-close" class="ant-btn css-var-r4c ant-btn-icon-sm" />
                                    <el-button type="primary" icon="el-icon-top" class="ant-btn css-var-r4c ant-btn-icon-sm" circle />
                                </div>
                            </div>
                        </div>
                    </div>
                </el-footer>
            </el-container>
        </el-container>
    </el-main>
</template>

<script>
export default {
    name: 'dashboard',
    components: {},
    data() {
        return {
            text: 'Hello World',
        };
    },
};
</script>

<style>
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
.ant-bubble {
    display: flex;
    column-gap: var(--ant-padding-sm);
    max-width: 100%;
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
}
.ant-bubble .ant-bubble-content-wrapper {
    flex: auto;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
}
.ant-bubble .ant-bubble-content-filled {
    background-color: var(--ant-color-fill-content);
}
.ant-bubble .ant-bubble-content-filled,
.ant-bubble .ant-bubble-content-outlined,
.ant-bubble .ant-bubble-content-shadow {
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
.ant-sender {
    position: relative;
    width: 100%;
    box-sizing: border-box;
    box-shadow: var(--ant-box-shadow-tertiary);
    transition: background var(--ant-motion-duration-slow);
    border-radius: calc(var(--ant-border-radius) * 2);
    border-color: var(--ant-color-border);
    border-width: 1px;
    border-style: solid;
}
.ant-sender:hover {
    box-shadow: var(--ant-color-info-hover);
}
.ant-sender .ant-sender-header {
    border-bottom-width: var(--ant-line-width);
    border-bottom-style: solid;
    border-bottom-color: var(--ant-color-border);
}
.ant-sender .ant-sender-header-header {
    background: var(--ant-color-fill-alter);
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    padding-block: calc(var(--ant-padding-sm) - var(--ant-line-width-bold));
    padding-inline-start: var(--ant-padding);
    padding-inline-end: var(--ant-padding-xs);
    display: flex;
}
.ant-sender-header-title {
    flex: auto;
}
.css-var-R9alfmckma.ant-typography {
    --ant-typography-title-margin-top: 1.2em;
    --ant-typography-title-margin-bottom: 0.5em;
}
.ant-typography.ant-typography-secondary {
    color: var(--ant-color-text-description);
}
.ant-sender .ant-sender-content {
    display: flex;
    gap: var(--ant-padding-xs);
    width: 100%;
    padding-block: var(--ant-padding-sm);
    padding-inline-start: var(--ant-padding);
    padding-inline-end: var(--ant-padding-sm);
    box-sizing: border-box;
    align-items: flex-end;
}
.ant-sender .ant-sender-prefix {
    flex: none;
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
.ant-sender .ant-sender-actions-list {
    flex: none;
    display: flex;
}
.ant-sender .ant-sender-actions-list-presets {
    gap: var(--ant-padding-xs);
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
.ant-sender .ant-sender-input {
    padding: 0;
    border-radius: 0;
    flex: auto;
    align-self: center;
    min-height: auto;
}
.ant-input-textarea textarea {
    box-shadow: none;
}
.ant-input-textarea textarea:hover {
    box-shadow: none;
}

.el-button + .el-button {
    margin-left: 5px;
}

.ant-conversations {
    display: flex;
    flex-direction: column;
    gap: var(--ant-padding-xxs);
    overflow-y: auto;
    padding: var(--ant-padding-sm);
}
.ant-conversations .ant-conversations-group-title {
    display: flex;
    align-items: center;
    height: var(--ant-control-height-lg);
    min-height: var(--ant-control-height-lg);
    padding: 0 var(--ant-padding-xs);
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
.ant-conversations .ant-conversations-list {
    display: flex;
    gap: var(--ant-padding-xxs);
    flex-direction: column;
}
.ant-conversations .ant-conversations-list .ant-conversations-item {
    padding-inline-start: var(--ant-padding-xl);
}
.ant-conversations .ant-conversations-item {
    display: flex;
    height: var(--ant-control-height-lg);
    min-height: var(--ant-control-height-lg);
    gap: var(--ant-padding-xs);
    padding: 0 var(--ant-padding-xs);
    align-items: center;
    border-radius: var(--ant-border-radius-lg);
    cursor: pointer;
    transition: all var(--ant-motion-duration-mid) var(--ant-motion-ease-in-out);
}
.ant-conversations .ant-conversations-list .ant-conversations-label {
    color: var(--ant-color-text-description);
}
</style>
