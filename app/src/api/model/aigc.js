import config from '@/config';
import http from '@/utils/request';

export default {
    agent: {
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
                return await http.post(
                    `${this.url}?conversationId=${params.id}`
                );
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
    },
    space: {
        list: {
            url: `${config.API_URL}/space/getList`,
            name: 'getList',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/space/get`,
            name: 'get',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/space/create`,
            name: 'create',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/space/update`,
            name: 'update',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/space/delete`,
            name: 'delete',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    model: {
        list: {
            url: `${config.API_URL}/model/getList`,
            name: 'getList',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/model/get`,
            name: 'get',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/model/create`,
            name: 'create',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/model/update`,
            name: 'update',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/model/delete`,
            name: 'delete',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    modelInstance: {
        list: {
            url: `${config.API_URL}/modelInstance/getList`,
            name: 'getList',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/modelInstance/get`,
            name: 'get',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/modelInstance/create`,
            name: 'create',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/modelInstance/update`,
            name: 'update',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/modelInstance/delete`,
            name: 'delete',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
        setIsDefault: {
            url: `${config.API_URL}/modelInstance/setIsDefault`,
            name: 'setIsDefault',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        setKey: {
            url: `${config.API_URL}/modelInstance/setKey`,
            name: 'setKey',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
    },

    knowledge: {
        list: {
            url: `${config.API_URL}/knowledge/getList`,
            name: 'getList',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/knowledge/get`,
            name: 'get',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/knowledge/create`,
            name: 'create',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/knowledge/update`,
            name: 'update',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/knowledge/delete`,
            name: 'delete',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    knowledgeDocument: {
        list: {
            url: `${config.API_URL}/knowledgeDocument/getList`,
            name: 'getList',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/knowledgeDocument/get`,
            name: 'get',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/knowledgeDocument/create`,
            name: 'create',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/knowledgeDocument/update`,
            name: 'update',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/knowledgeDocument/delete`,
            name: 'delete',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
};
