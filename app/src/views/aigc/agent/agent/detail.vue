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
                    states: '',
                },
            },
            agentId: this.$route.query.id,
            states: {},
            addTemplate: {},
            msgList: [],
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
                    this.states &&
                    this.states.llmStepState &&
                    this.states.llmStepState.chatMessages &&
                    this.states.llmStepState.chatMessages.length > 0 &&
                    this.states.llmStepState.chatMessages[0].content
                ) {
                    return this.states.llmStepState.chatMessages[0].content;
                }
                return '';
            },
            set(newContent) {
                if (!this.states) {
                    this.states = {};
                }
                if (!this.states.llmStepState) {
                    this.states.llmStepState = {};
                }
                if (!this.states.llmStepState.chatMessages) {
                    this.states.llmStepState.chatMessages = [];
                }
                if (this.states.llmStepState.chatMessages.length === 0) {
                    this.states.llmStepState.chatMessages.push({
                        role: 'system',
                        content: '',
                    });
                }
                this.states.llmStepState.chatMessages[0].content = newContent;
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
            if (this.form.states) {
                this.states = JSON.parse(this.form.states);
            }
        },
        async handleSave() {
            this.form.states = JSON.stringify(this.states);

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
