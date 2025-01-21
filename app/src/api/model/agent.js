import config from '@/config';
import http from '@/utils/request';

export default {
    list: {
        url: `${config.API_URL}/agent/getList`,
        name: 'getList',
        post: async function (params) {
            return await http.post(this.url, params);
        },
    },
    detail: {
        url: `${config.API_URL}/agent/get`,
        name: 'get',
        post: async function (params) {
            return await http.post(`${this.url}?id=${params.id}`);
        },
    },
    create: {
        url: `${config.API_URL}/agent/create`,
        name: 'create',
        post: async function (data = {}) {
            return await http.post(this.url, data);
        },
    },
    update: {
        url: `${config.API_URL}/agent/update`,
        name: 'update',
        post: async function (data) {
            return await http.post(`${this.url}?id=${data.id}`, data);
        },
    },
    getLlmAgent: {
        url: `${config.API_URL}/agent/getLlmAgent`,
        name: 'getLlmAgent',
        post: async function (params) {
            return await http.post(`${this.url}?id=${params.id}`);
        },
    },
    updateLlmAgent: {
        url: `${config.API_URL}/agent/updateLlmAgent`,
        name: 'updateLlmAgent',
        post: async function (data) {
            return await http.post(`${this.url}?id=${data.id}`, data);
        },
    },
    delete: {
        url: `${config.API_URL}/agent/delete`,
        name: 'delete',
        post: async function (id) {
            return await http.post(`${this.url}?id=${id}`);
        },
    },
    getAgentConversations: {
        url: `${config.API_URL}/agent/getAgentConversations`,
        name: 'getAgentConversations',
        post: async function (params) {
            return await http.post(`${this.url}?agentId=${params.id}`);
        },
    },
    updateConversationTitle: {
        url: `${config.API_URL}/agent/updateConversationTitle`,
        name: 'updateConversationTitle',
        post: async function (data) {
            return await http.post(`${this.url}`, data);
        },
    },
    deleteConversation: {
        url: `${config.API_URL}/agent/deleteConversation`,
        name: 'deleteConversation',
        post: async function (params) {
            return await http.post(`${this.url}?conversationId=${params.id}`);
        },
    },
    getAgentMessages: {
        url: `${config.API_URL}/agent/getAgentMessages`,
        name: 'getAgentMessages',
        post: async function (params) {
            return await http.post(
                `${this.url}?agentConversationId=${params.id}`
            );
        },
    },
};
