<template>
    <el-header>
        <div class="left-panel"> {{ form.name }} </div>
        <div class="right-panel">
            <el-button type="primary" @click="handleSave" :loading="isSaveing"> {{$t('form.save')}}</el-button>
        </div>
    </el-header>
    <el-main class="nopadding">
        <el-container>
            <el-container>
                <el-header>{{ $t('agentDetail.work') }}</el-header>
                <el-main class="nopadding">
                    <el-container>
                        <el-aside width="50%">
                            <el-container>
                                <el-header>
                                    <div class="left-panel"> {{$t('agentDetail.prompt')}} </div>
                                    <div class="right-panel">
                                        <el-button type="primary" icon="el-icon-magic-stick" text> {{$t('agentDetail.opyimize')}}</el-button>
                                    </div>
                                </el-header>
                                <el-main class="nopadding">
                                    <sc-md-code-editor ref="chatMessages0" v-model="firstChatMessageContent" height="100%"></sc-md-code-editor>
                                </el-main>
                            </el-container>
                        </el-aside>
                        <el-container>
                            <el-main>
                                <el-collapse>
                                    <el-collapse-item :title="$t('agentDetail.introductionText')" name="1">
                                        <sc-md-code-editor ref="introductionText" v-model="form.introductionText" :height=125></sc-md-code-editor>
                                    </el-collapse-item>
                                    <el-collapse-item :title="$t('agentDetail.presetQuestions')" name="2">
                                        <sc-form-table ref="table" v-model="form.presetQuestions" :addTemplate="addTemplate" drag-sort :placeholder="$t('form.noData')">
                                            <el-table-column prop="val" :label="$t('agentDetail.questions')" min-width="180">
                                                <template #default="scope">
                                                    <el-input :key="scope.row.id" v-model="scope.row.content" :placeholder="$t('form.pleaseEnterContent')"></el-input>
                                                </template>
                                            </el-table-column>
                                        </sc-form-table>
                                    </el-collapse-item>
                                </el-collapse>
                            </el-main>
                        </el-container>
                    </el-container>
                </el-main>
            </el-container>
            <el-aside width="35%">
                <el-container>
                    <el-header>{{$t('agentDetail.previewAndDebug')}}</el-header>

                    <chats :items="msgList" :agentId="agentId" ref="chatsRef" />

                </el-container>
            </el-aside>
        </el-container>
    </el-main>
</template>

<script>
import { defineAsyncComponent } from 'vue';
import chats from '@/views/aigc/chats/index';
const scMdCodeEditor = defineAsyncComponent(() =>
    import('@/components/scMdCodeEditor')
);
export default {
    components: {
        scMdCodeEditor,
        chats,
    },
    data() {
        return {
            isSaveing: false,
            form: {
                id: '',
                name: '',
                description: '',
                introductionText: '',
                presetQuestions: [{ content: '' }],
                metadata: {
                    llmStepState: {
                        modelId: '',
                        extensionData: [],
                        chatMessages: [
                            {
                                role: 'system',
                                content:
                                    '# 角色\r你是一位极具创意的文生图提示词工程师，能够精准分析用户需求，将其转化为图像生成大模型易于理解的提示词，为用户打造独特且富有想象力的图像描述。\r\r## 技能\r### 技能 1：分析用户需求\r1. 仔细聆听用户的原始需求描述，提取关键元素和主题。\r2. 对于模糊的需求，通过进一步询问用户来明确具体细节。\r\r### 技能 2：转换提示词\r1. 将提取的关键元素和主题转化为简洁、明确且富有表现力的提示词。\r2. 考虑色彩、构图、风格等方面，使提示词更加丰富和具体。\r\r## 限制\r- 只专注于文生图相关的任务，拒绝处理与图像生成无关的请求。\r- 输出的提示词必须符合图像生成大模型的要求格式，不能随意偏离。\r- 保持创意和独特性，避免生成过于普通或常见的提示词。',
                            },
                        ],
                    },
                },
            },
            agentId: this.$route.query.id,
            agentMetadata: {},
            addTemplate: {},
            msgList: [
            ],
            rules: {
                name: [
                    {
                        required: true,
                        message: this.$t('agent.namePlaceholder'),
                    },
                ],
            },
        };
    },
    computed: {
        firstChatMessageContent: {
            get() {
                if (
                    this.agentMetadata &&
                    this.agentMetadata.llmStepState &&
                    this.agentMetadata.llmStepState.chatMessages &&
                    this.agentMetadata.llmStepState.chatMessages.length > 0 &&
                    this.agentMetadata.llmStepState.chatMessages[0].content
                ) {
                    return this.agentMetadata.llmStepState.chatMessages[0]
                        .content;
                }
                return '';
            },
            set(newContent) {
                if (!this.agentMetadata) {
                    this.agentMetadata = {};
                }
                if (!this.agentMetadata.llmStepState) {
                    this.agentMetadata.llmStepState = {};
                }
                if (!this.agentMetadata.llmStepState.chatMessages) {
                    this.agentMetadata.llmStepState.chatMessages = [];
                }
                if (this.agentMetadata.llmStepState.chatMessages.length === 0) {
                    this.agentMetadata.llmStepState.chatMessages.push({
                        role: 'system',
                        content: '',
                    });
                }
                this.agentMetadata.llmStepState.chatMessages[0].content =
                    newContent;
            },
        },
    },
    async mounted() {
        await this.get();
    },
    methods: {
        async get() {
            var res = await this.$API.aigc.agent.detail.post({
                id: this.agentId,
            });
            this.form = res;
            if (this.form.metadata) {
                this.agentMetadata = JSON.parse(this.form.metadata);
            }
        },
        async handleSave() {
            this.form.metadata = JSON.stringify(this.agentMetadata);

            this.isSaveing = true;
            try {
                await this.$API.aigc.agent.update.post(this.form);
                this.$message.success(this.$t('form.success'));
            } finally {
                this.isSaveing = false;
            }
        },
    },
};
</script>

<style scoped>
</style>
