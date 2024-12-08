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
};
