import * as React from 'react';
import Layout from '../components/layoutempty';
import { graphql } from 'gatsby';
import BotDetailManger from '../components/views/bot/detail/detail';
import { BrowserRouter } from 'react-router-dom';

// markup
const BotPage = ({ data }: any) => {
    return (
        <BrowserRouter>
            <Layout
                meta={data.site.siteMetadata}
                title="Home"
                link={'/botdetail'}
            >
                <main style={{ height: '100%' }} className=" h-full ">
                    <BotDetailManger />
                </main>
            </Layout>
        </BrowserRouter>
    );
};

export const query = graphql`
    query ($language: String!) {
        site {
            siteMetadata {
                description
                title
            }
        }
        locales: allLocale(
            filter: { ns: { in: ["bot"] }, language: { eq: $language } }
        ) {
            edges {
                node {
                    ns
                    data
                    language
                }
            }
        }
    }
`;

export default BotPage;
