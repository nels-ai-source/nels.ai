<template>
    <el-main class="nopadding">
        <el-scrollbar ref="scrollbarRef">
            <ul ref="chatListRef" class="chat-list">
                <li v-for="item in messages" v-bind:key="item.id">
                    <user :key="item.id" v-if="item.role=='user'" :item="item"></user>
                    <assistant :key="item.id" v-if="item.role=='assistant'" :item="item"></assistant>
                </li>
                <el-empty v-if="items.length==0" :description="$t('chats.noData')" :image-size="100"></el-empty>
            </ul>
        </el-scrollbar>

    </el-main>
    <el-footer>
        <div>

        </div>
        <div>
            <el-input v-model="userPrompt" placeholder="Please input">
                <template #append><el-button icon="el-icon-comment" @click="handleSendMessage" /></template>
            </el-input>
        </div>
    </el-footer>

</template>

<script>
import { nextTick, reactive } from 'vue';
import { fetchEventSource } from '@microsoft/fetch-event-source';
import config from '@/config';
import tool from '@/utils/tool';
import user from './user';
import assistant from './assistant';

export default {
    name: 'application',
    components: {
        user,
        assistant,
    },
    props: {
        items: {
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
            messages: reactive([...this.items]),
            userPrompt: '',
        };
    },
    methods: {
        async handleSendMessage() {
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
                onopen(response) {
                    console.log('onopen' + response);
                    me.messages.push({
                        role: 'user',
                        content: me.userPrompt,
                    });
                },
                onmessage(ev) {
                    console.log('onmessage' + ev);
                    if (ev.event == 'message_template') {
                        let data = JSON.parse(ev.data);
                        me.messages.push({
                            role: 'assistant',
                            content: '',
                            template: data.Content,
                            id: data.StepId,
                        });
                    } else if (ev.event == 'data') {
                        let data = JSON.parse(ev.data);
                        let targetIndex = me.messages.findIndex(
                            (item) => item.id == data.StepId
                        );
                        if (targetIndex !== -1) {
                            me.messages[targetIndex].content =
                                me.messages[targetIndex].content + data.Content;
                            console.log(me.messages[targetIndex].content);
                        }
                        //  me.$forceUpdate();
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
        scrollToBottom() {
            nextTick(() => {
                const scrollbarRef = this.$refs.scrollbarRef;
                const chatListRef = this.$refs.chatListRef;
                if (scrollbarRef && chatListRef) {
                    scrollbarRef.setScrollTop(chatListRef.clientHeight);
                }
            });
        },
    },
    watch: {
        items(val) {
            this.messages = val;
        },
        messages(val) {
            this.$emit('update:messages', val);
        },
    },
};
</script>

<style scoped>
.chat-list li {
    display: flex;
    padding: 10px;
}
.chat-list__icon {
    width: 32px;
    margin-right: 15px;
}
.chat-list__main {
    flex: 1;
}
.chat-list__content {
    background-color: rgba(148, 152, 247, 0.44);
    border-radius: 8px;
    padding: 12px 16px;
}

.chat-list__main h2 {
    font-size: 15px;
    font-weight: normal;
    color: #333;
}
.chat-list__main p {
    font-size: 12px;
    line-height: 1.8;
}
.chat-list__main p h1 {
    font-size: 12px;
    line-height: 1.8;
}
.chat-list__time {
    width: 100px;
    text-align: right;
    color: #999;
}

.dark .chat-list__main h2 {
    color: #d0d0d0;
}
.dark .chat-list li {
    border-top: 1px solid #363636;
}
.dark .chat-list li a:hover {
    background: #383838;
}
</style>
